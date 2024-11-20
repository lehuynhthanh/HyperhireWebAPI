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

public class GetProductDetailQuery : IRequest<Response<ProductDetailDto>>
{
    public Guid ProductId { get; set; }
}

public class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, Response<ProductDetailDto>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly ILogger<GetProductDetailQueryHandler> _logger;

    public GetProductDetailQueryHandler(
        IGenericRepository<ProductDetail, Guid> productDetailRepository,
        IMapper mapper,
        ILogger<GetProductDetailQueryHandler> logger)
    {
        _productDetailRepository = productDetailRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<ProductDetailDto>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {

            var resultDtos = await _productDetailRepository.GetAll().AsNoTracking().ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x => x.Id == request.ProductId);
            return Response<ProductDetailDto>.Success(resultDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<ProductDetailDto>.Failure(null, new List<string> { ex.Message });
        }
    }
}
