using HyperhireWebAPI.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HyperhireWebAPI.Infrastructure;

public class JwtService
{
    private readonly string _secretKey = "612446ebd0ed897b5b96431b94b6162e9e1d7efa8f39ba6db267e9839200584a"; // Change this key

    public string GenerateJwtToken(Claim[] claim)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "your-app",
            audience: "your-app",
            claims: claim,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}