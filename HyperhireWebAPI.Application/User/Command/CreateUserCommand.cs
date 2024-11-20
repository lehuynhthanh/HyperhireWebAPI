using AutoMapper;
using AutoMapper.QueryableExtensions;
using HyperhireWebAPI.Application.CategoryApplication.Dto;
using HyperhireWebAPI.Application.Common.Handle;
using HyperhireWebAPI.Application.Common.Models;
using HyperhireWebAPI.Application.OrderApplication.Dto;
using HyperhireWebAPI.Application.ProductDetailApplication.Dto;
using HyperhireWebAPI.Application.User.Dto;
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

namespace HyperhireWebAPI.Application.User.Command;
public class CreateUserCommand : IRequest<Response<UserDto>>
{
    [Required(ErrorMessage = "Please fill in your name.")]
    [MaxLength(50)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Please fill in your password.")]
    [MaxLength(50)]
    public string Password { get; set; }
}

public class CreateUserCommandHandler : BaseHandler<CreateUserCommand, Response<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<ProductDetail, Guid> _productDetailRepository;
    private readonly IGenericRepository<HyperhireWebAPI.Domain.Entities.User, Guid> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<Category, Guid> _categoryRepository;

    public CreateUserCommandHandler(
        IMapper mapper,
        ILogger<CreateUserCommandHandler> logger,
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
        _unitOfWork = _userRepository.UnitOfWork;
    }

    public override async Task<Response<UserDto>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userCheck = await _userRepository.GetAll().AsNoTracking().AnyAsync(x => x.UserName == command.UserName);
            if (userCheck)
                throw new Exception("Already has this user name!");

            var user = new HyperhireWebAPI.Domain.Entities.User
            {
                UserName = command.UserName,
                Password = command.Password,
                CreatedBy = "Register"
            };
            await _userRepository.AddAsync(user, cancellationToken);

            var response = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (response > 0)
            {
                return Response<UserDto>.Success(_mapper.Map<UserDto>(user));
            }
            else
            {
                throw new Exception("Create user Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Response<UserDto>.Failure(null, new List<string> { ex.Message });
        }
    }
}
