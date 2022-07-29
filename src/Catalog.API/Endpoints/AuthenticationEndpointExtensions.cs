using Catalog.API.Models;
using Catalog.API.Services.Contracts;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.API.Endpoints
{
    public static class AuthenticationEndpointExtensions
    {
        public static void MapAuthenticationEndpoints(this WebApplication app, IConfiguration configuration)
        {
            app.MapPost("/login", [AllowAnonymous] (ITokenService tokenService, User user) =>
            {
                var tokenSettings = configuration.Get<JwtSettings>();

                if (user is null)
                    return Results.BadRequest("Invalid login.");

                if (user.UserName == "developer" && user.Password == "passwd")
                {
                    var token = tokenService.GenerateToken(tokenSettings.Key, tokenSettings.Issuer, tokenSettings.Audience, user);

                    return Results.Ok(new { token });
                }

                return Results.BadRequest("Invalid login.");
            }).Produces(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status400BadRequest)
              .WithName("Login")
              .WithTags("Authentication");
        }
    }
}
