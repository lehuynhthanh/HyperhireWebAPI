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
using System.Security.Claims;

namespace HyperhireWebAPI.Application.OrderApplication.Command;
public class GetOrdersByUserQuery : IRequest<Response<List<OrderDto>>>
{
}

public class GetOrdersByUserQueryHandler : BaseHandler<GetOrdersByUserQuery, Response<List<OrderDto>>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<OrderDetail, Guid> _orderDetailRepository;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrdersByUserQueryHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetOrdersByUserQueryHandler(
        IGenericRepository<OrderDetail, Guid> orderDetailRepository,
        IMapper mapper,
        ILogger<GetOrdersByUserQueryHandler> logger,
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

    public override async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserQuery command, CancellationToken cancellationToken)
    {
        try
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var user = await _userRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.UserName == userName);

            var orders = await _orderDetailRepository.GetAll().Where(x => x.UserId == user.Id).Include(order => order.User).Include(order => order.Products).AsNoTracking().ToListAsync();
            var listOrders = new List<OrderDto>();
            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.Product = _mapper.Map<ProductDetailDto>(order.Products);
                listOrders.Add(orderDto);
            }

            return Response<List<OrderDto>>.Success(listOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<List<OrderDto>>.Failure(null, new List<string> { ex.Message });
        }
    }
}
