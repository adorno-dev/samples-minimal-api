using Catalog.API.Contexts;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Endpoints
{
    public static class ProductEndpointExtensions
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync())
               .Produces<IList<Product>>(StatusCodes.Status200OK)
               .WithTags("Products");

            app.MapGet("/products/{id:int}", async (AppDbContext db, int id) =>
            {
                return await db.Products.FindAsync(id) is Product product ?
                    Results.Ok(product) :
                    Results.NotFound("Product not found.");
            }).Produces<Product>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound)
              .WithTags("Products");

            app.MapGet("/products/{name}", async (AppDbContext db, string name) =>
            {
                var products = await db.Products.AsNoTracking()
                                                .Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(name.ToLower()))
                                                .ToListAsync();

                return products.Any() ?
                    Results.Ok(products) :
                    Results.NotFound(Array.Empty<Product>());

            }).Produces<IList<Product>>(StatusCodes.Status200OK)
              .WithName("ProductsByName")
              .WithTags("Products");

            app.MapGet("/products/pages", async (AppDbContext db, int pageNumber, int pageLength) =>
            {
                return await db.Products.AsNoTracking()
                                        .Skip((pageNumber - 1) * pageLength)
                                        .Take(pageLength)
                                        .ToListAsync();

            }).Produces<IList<Product>>(StatusCodes.Status200OK)
              .WithName("PaginatedProducts")
              .WithTags("Products");

            app.MapPost("/products", async (AppDbContext db, Product product) =>
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return Results.Created($"/products/{product.ProductId}", product);
            }).Produces<Product>(StatusCodes.Status201Created)
              .WithName("AddNewProduct")
              .WithTags("Products");

            app.MapPut("/products", async (AppDbContext db, int id, string name, string description) =>
            {
                var storedProduct = await db.Products.FindAsync(id);

                if (storedProduct is null)
                    return Results.NotFound("Product not found.");

                storedProduct.Name = name;
                storedProduct.Description = description;

                await db.SaveChangesAsync();

                return Results.Ok(storedProduct);
            }).Produces<Product>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound)
              .WithName("UpdateProductNameOrDescription")
              .WithTags("Products");

            app.MapPut("/products/{id:int}", async (AppDbContext db, Product product, int id) =>
            {
                if (!product.ProductId.Equals(id))
                    return Results.BadRequest("ProductId and Id mismatch.");

                if (!db.Products.Any(p => p.ProductId.Equals(product.ProductId)))
                    return Results.NotFound("Product not found.");

                db.Entry<Product>(product).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Results.Ok(product);
            }).Produces<Product>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound)
              .Produces(StatusCodes.Status400BadRequest)
              .WithName("UpdateProduct")
              .WithTags("Products");

            app.MapDelete("/products/{id:int}", async (AppDbContext db, int id) =>
            {
                var storedProduct = await db.Products.FindAsync(id);

                if (storedProduct is null)
                    return Results.NotFound("Product not found.");

                db.Products.Remove(storedProduct);

                await db.SaveChangesAsync();

                return Results.Ok(storedProduct);

            }).Produces<Product>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound)
              .WithName("DeleteProduct")
              .WithTags("Products");
        }
    }
}
