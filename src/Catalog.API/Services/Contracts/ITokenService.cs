using Catalog.API.Models;

namespace Catalog.API.Services.Contracts
{
    public interface ITokenService
    {
        string GenerateToken(string key, string issuer, string audience, User user);
    }
}
