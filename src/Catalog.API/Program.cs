using Catalog.API.Endpoints;
using Catalog.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var settings = new
{
    ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection"),
    TokenSettings = builder.Configuration.GetSection("JwtSettings")
};

builder.Services.AddSwagger();
builder.Services.AddPersistence(settings.ConnectionString);
builder.Services.AddJwtAuthentication(settings.TokenSettings);
builder.Services.AddApiServices();

var app = builder.Build();

app.MapAuthenticationEndpoints(settings.TokenSettings);
app.MapCategoryEndpoints();
app.MapProductEndpoints();

app.UseApiSwagger();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.Run();

public partial class Program { }