using HyperhireWebAPI.Application.Common.Models;
using HyperhireWebAPI.Application.OrderApplication.Command;
using HyperhireWebAPI.Application.OrderApplication.Dto;
using HyperhireWebAPI.Application.ProductApplication.Command;
using HyperhireWebAPI.Application.ProductDetailApplication.Dto;
using HyperhireWebAPI.Application.ProductDetailApplication.Queries;
using HyperhireWebAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HyperhireWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Response<ProductDetailDto>>> Post([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ProductDetailDto>>> Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductDetailQuery { ProductId = id }, cancellationToken);
            return Ok(result);
        }

        // GET: api/Gets
        [HttpGet]
        public async Task<ActionResult<Response<List<ProductDetailDto>>>> Gets([FromQuery] GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
