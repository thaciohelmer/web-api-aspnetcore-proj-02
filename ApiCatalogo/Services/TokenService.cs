﻿using ApiCatalogo.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCatalogo.Services;


public class TokenService : ITokensService
{
    public string GerarToken(string key, string issue, string audience, UserModel user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };
        
        var securityKEy = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKEy, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: issue, audience: audience, claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}
