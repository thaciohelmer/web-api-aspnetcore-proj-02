using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiCatalogo.ApiEndpoints;

public static class AutenticacaoEndpoint
{
    public static void MapAutenticacaoEndpoint(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokensService tokenService) =>
        {
            if (userModel == null)
            {
                return Results.BadRequest("Login Inválido");
            }
            if (userModel.UserName == "thaciohelmer" && userModel.Password == "helmerthacio123")
            {
                var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"],
                    app.Configuration["Jwt:Audience"],
                    userModel);
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Login Inválido");
            }
        }).Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status200OK)
              .WithName("Login")
              .WithTags("Autenticação");
    }
}
