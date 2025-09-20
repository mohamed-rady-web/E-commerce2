using Ecommerce.Dtos.User.Reviews;

namespace Ecommerce.Dtos.Products
{
    public class ProductDto
    {
        public string Message { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CoverPhotoUrl { get; set; }
        public string SinglePhotoUrl { get; set; }
        public string[] ImageUrl { get; set; }
        public int Stock { get; set; }
        public double Rating { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? SpecialOfferId { get; set; }
        public string? SpecialOfferName { get; set; }
        public ICollection<ReviewsDto> Reviews { get; set; }

        }
}
