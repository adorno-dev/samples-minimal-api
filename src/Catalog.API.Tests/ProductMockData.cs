using Catalog.API.Contexts;
using Catalog.API.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Tests
{
    public class ProductMockData
    {
        public static async Task CreateProducts(CatalogApplicationFactory catalogApplicationFactory, bool creation)
        {
            using var scope = catalogApplicationFactory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.EnsureCreatedAsync();

            if (creation)
            {
                await context.Products.AddRangeAsync(new Product[]
                {
                    new Product(1, "Product #1", "Product #1 Description", "product-1.png", 10, DateTime.Now),
                    new Product(2, "Product #2", "Product #2 Description", "product-2.png", 20, DateTime.Now)
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
