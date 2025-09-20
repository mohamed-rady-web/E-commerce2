using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos.Products
{
    public class AddProductDto
    {
        [Required, MaxLength(100),MinLength(10),]
        public string Name { get; set; }
        [Required, MaxLength(100), MinLength(10)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required, DataType(DataType.ImageUrl)]
        public string CoverPhotoUrl { get; set; }
        [Required, DataType(DataType.ImageUrl)]
        public string SinglePhotoUrl { get; set; }
        [Required, DataType(DataType.ImageUrl)]
        public string[] ImageUrl { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public double Rating { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int? SpecialOfferId { get; set; }
    }
}
