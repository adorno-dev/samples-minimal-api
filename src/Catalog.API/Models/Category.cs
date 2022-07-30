using System.Text.Json.Serialization;

namespace Catalog.API.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        //[JsonIgnore]
        public ICollection<Product>? Products { get; set; }

        public Category() {}

        public Category(int categoryId, string? name, string? description)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
        }
    }
}
