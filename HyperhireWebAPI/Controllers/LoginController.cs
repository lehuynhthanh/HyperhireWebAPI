using HyperhireWebAPI.Application.Common.Models;
using HyperhireWebAPI.Application.OrderApplication.Command;
using HyperhireWebAPI.Application.OrderApplication.Dto;
using HyperhireWebAPI.Application.User.Command;
using HyperhireWebAPI.Application.User.Dto;
using HyperhireWebAPI.Domain;
using HyperhireWebAPI.Domain.Entities;
using HyperhireWebAPI.Domain.Repositories;
using HyperhireWebAPI.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HyperhireWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly HyperhireDbContext _context;
    private readonly IMediator _mediator;

    public AuthController(JwtService jwtService, HyperhireDbContext context, IMediator mediator)
    {
        _jwtService = jwtService;
        _context = context;
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        // In real-world scenarios, validate against a database
        var user = await _context.User.AsNoTracking().SingleOrDefaultAsync(x => x.UserName == loginRequest.UserName);


        if (user?.UserName == loginRequest.UserName && user?.Password == loginRequest.Password)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var token = _jwtService.GenerateJwtToken(claims);
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid credentials");
    }

    [HttpPost]
    public async Task<ActionResult<Response<UserDto>>> Post([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

public class LoginRequest
{
    public string UserName { get; set; }

    public string Password { get; set; }
}
