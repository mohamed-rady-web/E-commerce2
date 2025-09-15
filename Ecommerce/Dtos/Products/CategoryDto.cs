using Ecommerce.Models.Product;

namespace Ecommerce.Dtos.Products
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Message { get; set; }
        public ICollection<ProductModel>Products { get; set; }
    }
}
