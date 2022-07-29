using Catalog.API.Contexts;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Endpoints
{
    public static class CategoryEndpointExtensions
    {
        public static void MapCategoryEndpoints(this WebApplication app)
        {
            app.MapGet("/categories", async (AppDbContext db) => await db.Categories.ToListAsync())
               .WithTags("Categories")
               .RequireAuthorization();

            app.MapGet("/categories/{id:int}", async (AppDbContext db, int id) =>
            {
                return await db.Categories.FindAsync(id) is Category category ?
                    Results.Ok(category) :
                    Results.NotFound();
            }).WithTags("Categories");

            app.MapPost("/categories", async (AppDbContext db, Category category) =>
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return Results.Created($"/categories/{category.CategoryId}", category);
            }).WithTags("Categories");

            app.MapPut("/categories/{id:int}", async (AppDbContext db, Category category, int id) =>
            {
                if (!category.CategoryId.Equals(id))
                    return Results.BadRequest();

                var storedCategory = await db.Categories.FindAsync(id);

                if (storedCategory is null)
                    return Results.NotFound();

                storedCategory.Name = category.Name;
                storedCategory.Description = category.Description;

                await db.SaveChangesAsync();

                return Results.Ok(storedCategory);
            }).WithTags("Categories");

            app.MapDelete("/categories/{id:int}", async (AppDbContext db, int id) =>
            {
                var storedCategory = await db.Categories.FindAsync(id);

                if (storedCategory is null)
                    return Results.NotFound();

                db.Categories.Remove(storedCategory);

                await db.SaveChangesAsync();

                return Results.NoContent();
            }).WithTags("Categories");

            app.MapGet("/categories/products", async (AppDbContext db) => await db.Categories.Include(c => c.Products).AsNoTracking().ToListAsync())
               .Produces<IList<Category>>(StatusCodes.Status200OK)
               .WithTags("Categories");
        }
    }
}
