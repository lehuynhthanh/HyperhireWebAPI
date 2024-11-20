using AutoMapper;
using AutoMapper.QueryableExtensions;
using HyperhireWebAPI.Application.CategoryApplication.Dto;
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
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace HyperhireWebAPI.Application.CategoryApplication.Command;
public class CreateCategoryCommand : IRequest<Response<CategoryDto>>
{
    [Required(ErrorMessage = "Please fill in CategoryNameId.")]
    public string CategoryNameId { get; set; }

    [Required(ErrorMessage = "Please fill in Name.")]
    public string Name { get; set; }

    public string? Icon { get; set; }

    public bool IsActive { get; set; } = false;
}

public class CreateCategoryCommandHandler : BaseHandler<CreateCategoryCommand, Response<CategoryDto>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<Category, Guid> _categoryRepository;

    public CreateCategoryCommandHandler(
        IMapper mapper,
        ILogger<CreateCategoryCommandHandler> logger,
        IGenericRepository<ProductDetail, Guid> productDetailRepository,
        IHttpContextAccessor httpContextAccessor,
        IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> userRepository,
        IGenericRepository<Category, Guid> categoryRepository)
    {
        _mapper = mapper;
        _logger = logger;
        _productDetailRepository = productDetailRepository;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = _categoryRepository.UnitOfWork;
    }

    public override async Task<Response<CategoryDto>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var user = await _userRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.UserName == userName);

            var category = new Category
            {
               CategoryNameId = command.CategoryNameId,
               Name = command.Name,
               Icon = command.Icon,
               IsActive = command.IsActive,
               CreatedBy = user.UserName
            };
            await _categoryRepository.AddAsync(category, cancellationToken);

            var response = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (response > 0)
            {
                return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category));
            }
            else
            {
                throw new Exception("Create Category Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<CategoryDto>.Failure(null, new List<string> { ex.Message });
        }
    }
}
