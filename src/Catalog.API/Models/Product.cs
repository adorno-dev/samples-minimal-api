using System.Text.Json.Serialization;

namespace Catalog.API.Models
{
    public class Product
    {
        public Product() {}

        public Product(int productId, string? name, string? description, string? image, int stock, DateTime soldDate)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Image = image;
            Stock = stock;
            SoldDate = soldDate;
        }

        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int Stock { get; set; }
        public DateTime SoldDate { get; set; }

        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
