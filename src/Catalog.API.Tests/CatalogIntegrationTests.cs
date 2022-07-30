using Catalog.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace Catalog.API.Tests
{
    public class CatalogIntegrationTests
    {
        [Test]
        public async Task ShouldReturnAllProducts()
        {
            await using var app = new CatalogApplicationFactory();
            await ProductMockData.CreateProducts(app, true);

            var endpoint = "/products";
            var consumer = app.CreateClient();
            var response = await consumer.GetAsync(endpoint);
            var products = await consumer.GetFromJsonAsync<IList<Product>>(endpoint);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK));
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count.Equals(2));
        }

        [Test]
        public async Task ShouldReturnEmptyProducts()
        {
            await using var app = new CatalogApplicationFactory();
            await ProductMockData.CreateProducts(app, false);

            var consumer = app.CreateClient();
            var products = await consumer.GetFromJsonAsync<IList<Product>>("/products");

            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count.Equals(0));
        }

        [Test]
        public async Task ShouldReturnLoginBadRequest()
        {
            await using var app = new CatalogApplicationFactory();

            var user = new User("developer", "password");
            var consumer = app.CreateClient();
            var response = await consumer.PostAsJsonAsync("/login", user);

            Assert.IsTrue(HttpStatusCode.BadRequest.Equals(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ShouldReturnLoginOK()
        {
            await using var app = new CatalogApplicationFactory();

            var user = new User("developer", "passwd");
            var consumer = app.CreateClient();
            var response = await consumer.PostAsJsonAsync("/login", user);

            Assert.IsTrue(HttpStatusCode.OK.Equals(HttpStatusCode.OK));
        }

        [Test]
        public async Task ShouldReturnCategoriesUnauthorized()
        {
            await using var app = new CatalogApplicationFactory();
            await CatalogMockData.CreateCategories(app, true);

            var consumer = app.CreateClient();
            var response = await consumer.GetAsync("/categories");

            Assert.IsTrue(HttpStatusCode.Unauthorized.Equals(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task ShouldCreateNewProduct()
        {
            await using var app = new CatalogApplicationFactory();
            await ProductMockData.CreateProducts(app, true);

            var product = new Product(3, "Product #3", "Product #3 Description", "product-3.png", 300, DateTime.Now);
            var consumer = app.CreateClient();
            var response = await consumer.PostAsJsonAsync("/products", product);
            var products = await consumer.GetFromJsonAsync<IList<Product>>("/products");

            Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.Created));
            Assert.IsTrue(products?.Count.Equals(3));
        }
    }
}