using HyperhireWebAPI.Application.CategoryApplication.Command;
using HyperhireWebAPI.Application.CategoryApplication.Dto;
using HyperhireWebAPI.Application.CategoryApplication.Queries;
using HyperhireWebAPI.Application.Common.Models;
using HyperhireWebAPI.Application.OrderApplication.Command;
using HyperhireWebAPI.Application.OrderApplication.Dto;
using HyperhireWebAPI.Application.ProductApplication.Command;
using HyperhireWebAPI.Application.ProductDetailApplication.Dto;
using HyperhireWebAPI.Application.ProductDetailApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HyperhireWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Response<CategoryDto>>> Post([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    // GET: api/Gets
    [HttpGet]
    public async Task<ActionResult<Response<List<CategoryDto>>>> Gets(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery { }, cancellationToken);
        return Ok(result);
    }
}
