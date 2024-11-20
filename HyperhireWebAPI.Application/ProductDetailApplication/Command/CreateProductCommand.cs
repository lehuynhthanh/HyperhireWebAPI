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

namespace HyperhireWebAPI.Application.ProductApplication.Command;
public class CreateProductCommand : IRequest<Response<ProductDetailDto>>
{
    [Required(ErrorMessage = "Please fill in Name.")]
    [MaxLength(50)]
    public string Name { get; set; }

    public Guid CategoryId { get; set; }

    [Range(1, 999, ErrorMessage = "OriginalPrice must be greater than 0 and less than 999")]
    public decimal OriginalPrice { get; set; }

    [Range(1, 999, ErrorMessage = "Price must be greater than 0 and less than 999")]
    public decimal Price { get; set; }

    [Range(1, 10, ErrorMessage = "MaxNight must be greater than 0 and less than 10")]
    public int MaxNight { get; set; }

    [Range(1, 10, ErrorMessage = "MaxGuests must be greater than 0 and less than 10")]
    public int MaxGuests { get; set; }

    public List<string>? ImgUrl { get; set; }

    public string? Location { get; set; }

    public string? Decription { get; set; }
}

public class CreateProductCommandHandler : BaseHandler<CreateProductCommand, Response<ProductDetailDto>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<Category, Guid> _categoryRepository;

    public CreateProductCommandHandler(
        IMapper mapper,
        ILogger<CreateProductCommandHandler> logger,
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
        _unitOfWork = _productDetailRepository.UnitOfWork;
    }

    public override async Task<Response<ProductDetailDto>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var user = await _userRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.UserName == userName);

            var categoryDto = await _categoryRepository.GetAll().AsNoTracking().ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x => x.Id == command.CategoryId);
            if (categoryDto == null)
                throw new Exception("Category not available");

            var product = new ProductDetail
            {
                Name = command.Name,
                CategoryId = command.CategoryId,
                OriginalPrice = command.OriginalPrice,
                Price = command.Price,
                MaxNight = command.MaxNight,
                MaxGuests = command.MaxGuests,
                ImgUrl = command.ImgUrl,
                Location = command.Location,
                Decription = command.Decription,
                CreatedBy = user.UserName
            };
            await _productDetailRepository.AddAsync(product, cancellationToken);

            var response = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (response > 0)
            {
                return Response<ProductDetailDto>.Success(_mapper.Map<ProductDetailDto>(product));
            }
            else
            {
                throw new Exception("Create Order Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<ProductDetailDto>.Failure(null, new List<string> { ex.Message });
        }
    }
}
