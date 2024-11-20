using AutoMapper;
using AutoMapper.QueryableExtensions;
using HyperhireWebAPI.Application.Common.Handle;
using HyperhireWebAPI.Application.Common.Models;
using HyperhireWebAPI.Application.OrderApplication.Dto;
using HyperhireWebAPI.Application.ProductDetailApplication.Dto;
using HyperhireWebAPI.Domain.Constant;
using HyperhireWebAPI.Domain.Entities;
using HyperhireWebAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Security.Claims;

namespace HyperhireWebAPI.Application.OrderApplication.Command;
public class CreateOrderCommand : IRequest<Response<OrderDto>>
{
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.DateTime)]
    public DateTime CheckInDate { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.DateTime)]
    public DateTime CheckOutDate { get; set; }


    [Range(1, 10, ErrorMessage = "TotalGuests must be between 1 and 10")]
    public int TotalGuests { get; set; }

    [Range(0, 5, ErrorMessage = "TotalInfants must be between 0 and 5")]
    public int TotalInfants { get; set; }

    [Range(0, 5, ErrorMessage = "TotalPets must be between 0 and 5")]
    public int TotalPets { get; set; }
}

public class CreateOrderCommandHandler : BaseHandler<CreateOrderCommand, Response<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<OrderDetail, Guid> _orderDetailRepository;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateOrderCommandHandler(
        IGenericRepository<OrderDetail, Guid> orderDetailRepository,
        IMapper mapper,
        ILogger<CreateOrderCommandHandler> logger,
        IGenericRepository<ProductDetail, Guid> productDetailRepository,
        IHttpContextAccessor httpContextAccessor,
        IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> userRepository)
    {
        _orderDetailRepository = orderDetailRepository;
        _mapper = mapper;
        _logger = logger;
        _productDetailRepository = productDetailRepository;
        _unitOfWork = orderDetailRepository.UnitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public override async Task<Response<OrderDto>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var user = await _userRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.UserName == userName);

            var producttDto = await _productDetailRepository.GetAll().AsNoTracking().ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x => x.Id == command.ProductId);
            if (producttDto == null)
                throw new Exception("Product not available");

            var totalNights = (command.CheckOutDate - command.CheckInDate).Days;
            if (totalNights > producttDto.MaxNight)
                throw new Exception($"Cannot over {producttDto.MaxNight} nights");

            if (command.TotalGuests > producttDto.MaxGuests)
                throw new Exception($"Cannot over {producttDto.MaxGuests} guests");

            if (command.CheckInDate <= DateTime.Now.AddDays(1))
                throw new Exception($"Need next future date at least 1 day");

            if (command.CheckOutDate <= DateTime.Now.AddDays(1))
                throw new Exception($"Need next future date at least 1 day");

            if (command.CheckOutDate < command.CheckInDate.AddDays(1))
                throw new Exception($"CheckOutDate must over CheckInDate at least 1 day");

            var order = new OrderDetail
            {
                CheckInDate = command.CheckInDate,
                CheckOutDate = command.CheckOutDate,
                TotalNights = totalNights,
                TotalGuests = command.TotalGuests,
                TotalInfants = command.TotalInfants,
                TotalPets = command.TotalPets,
                Phone = user.UserName,
                ProductPrice = producttDto.Price,
                TotalPrice = producttDto.Price * totalNights,
                ProductId = producttDto.Id,
                OrderStatus = OrderStatus.Created,
                UserId = user.Id,
            };
            await _orderDetailRepository.AddAsync(order, cancellationToken);

            var response = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (response > 0)
            {
                return Response<OrderDto>.Success(_mapper.Map<OrderDto>(order));
            }
            else
            {
                throw new Exception("Create Order Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<OrderDto>.Failure(null, new List<string> { ex.Message });
        }
    }
}
