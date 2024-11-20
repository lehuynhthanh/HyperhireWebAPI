using AutoMapper;
using AutoMapper.QueryableExtensions;
using HyperhireWebAPI.Application.ProductDetailApplication.Dto;
using HyperhireWebAPI.Domain.Entities;
using HyperhireWebAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HyperhireWebAPI.Application.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace HyperhireWebAPI.Application.ProductDetailApplication.Queries;

public class GetProductsByCategoryQuery : IRequest<Response<List<ProductDetailDto>>>
{
    public Guid CategoryId { get; set; }
}

public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, Response<List<ProductDetailDto>>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly ILogger<GetProductDetailQueryHandler> _logger;

    public GetProductsByCategoryHandler(
        IGenericRepository<ProductDetail, Guid> productDetailRepository,
        IMapper mapper,
        ILogger<GetProductDetailQueryHandler> logger)
    {
        _productDetailRepository = productDetailRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<List<ProductDetailDto>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {

            var resultDtos = await _productDetailRepository.GetAll()
                .Where(x => x.CategoryId == request.CategoryId)
                .AsNoTracking().ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Response<List<ProductDetailDto>>.Success(resultDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<List<ProductDetailDto>>.Failure(null, new List<string> { ex.Message });
        }
    }
}
