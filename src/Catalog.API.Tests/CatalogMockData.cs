using Catalog.API.Contexts;
using Catalog.API.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Tests
{
    public class CatalogMockData
    {
        public static async Task CreateCategories(CatalogApplicationFactory catalogApplicationFactory, bool creation)
        {
            using var scope = catalogApplicationFactory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.EnsureCreatedAsync();

            if (creation)
            {
                await context.Categories.AddRangeAsync(new Category[]
                {
                    new Category(1, "Category #1", "Category #1 Described"),
                    new Category(2, "Category #2", "Category #2 Described"),
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
