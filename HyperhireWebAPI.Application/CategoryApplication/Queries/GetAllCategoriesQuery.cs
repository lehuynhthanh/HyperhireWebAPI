using AutoMapper;
using AutoMapper.QueryableExtensions;
using HyperhireWebAPI.Application.CategoryApplication.Dto;
using HyperhireWebAPI.Application.Common.Models;
using HyperhireWebAPI.Domain.Entities;
using HyperhireWebAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HyperhireWebAPI.Application.CategoryApplication.Queries;

public class GetAllCategoriesQuery : IRequest<Response<List<CategoryDto>>>
{
}

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Response<List<CategoryDto>>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<Category, Guid> _categoryRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

    public GetAllCategoriesQueryHandler(
        IGenericRepository<Category, Guid> categoryRepository,
        IMapper mapper,
        ILogger<GetAllCategoriesQueryHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {

            var resultDtos = await _categoryRepository.GetAll()
                .Where(x => x.IsActive)
                .AsNoTracking().ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Response<List<CategoryDto>>.Success(resultDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<List<CategoryDto>>.Failure(null, new List<string> { ex.Message });
        }
    }
}