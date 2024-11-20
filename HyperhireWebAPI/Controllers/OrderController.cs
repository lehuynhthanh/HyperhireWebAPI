using HyperhireWebAPI.Application.OrderApplication.Command;
using HyperhireWebAPI.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading;
using HyperhireWebAPI.Application.OrderApplication.Dto;
using HyperhireWebAPI.Application.Common.Models;
using HyperhireWebAPI.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HyperhireWebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Response<OrderDto>>> Post([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<OrderDto>>> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery { OrderId = id }, cancellationToken);
        return Ok(result);
    }

    // GET: api/Gets
    [HttpGet]
    public async Task<ActionResult<Response<List<OrderDto>>>> Gets(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrdersByUserQuery { }, cancellationToken);
        return Ok(result);
    }
}
