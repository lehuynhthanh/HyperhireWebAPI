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
using System.Security.Claims;

namespace HyperhireWebAPI.Application.OrderApplication.Command;
public class GetOrderByIdQuery : IRequest<Response<OrderDto>>
{
    public Guid OrderId { get; set; }
}

public class GetOrderByIdQueryHandler : BaseHandler<GetOrderByIdQuery, Response<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<OrderDetail, Guid> _orderDetailRepository;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrderByIdQueryHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetOrderByIdQueryHandler(
        IGenericRepository<OrderDetail, Guid> orderDetailRepository,
        IMapper mapper,
        ILogger<GetOrderByIdQueryHandler> logger,
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

    public override async Task<Response<OrderDto>> Handle(GetOrderByIdQuery command, CancellationToken cancellationToken)
    {
        try
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var user = await _userRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.UserName == userName);

            var order = await _orderDetailRepository.GetAll().Where(x => x.UserId == user.Id && x.Id == command.OrderId).Include(order => order.User).Include(order => order.Products).AsNoTracking().SingleOrDefaultAsync();
            if (order == null) 
            {
                throw new Exception("Cannot find this order");
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.Product = _mapper.Map<ProductDetailDto>(order.Products);

            return Response<OrderDto>.Success(orderDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<OrderDto>.Failure(null, new List<string> { ex.Message });
        }
    }
}
