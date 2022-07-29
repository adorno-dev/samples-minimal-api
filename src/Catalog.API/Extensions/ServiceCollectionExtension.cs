using Catalog.API.Contexts;
using Catalog.API.Services;
using Catalog.API.Services.Contracts;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Catalog.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = @"JWT authorization header using the bearer scheme. Enter 'Bearer'[space] <token>'",
                    In = ParameterLocation.Header
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        new string[] {}
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                var tokenSettings = configuration.Get<JwtSettings>();

                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,

                                    ValidIssuer = tokenSettings.Issuer,
                                    ValidAudience = tokenSettings.Audience,

                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key))
                                };
                            });
            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
            => services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSingleton<ITokenService, TokenService>();
            return services;
        }
    }
}
