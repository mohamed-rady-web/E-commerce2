namespace Ecommerce.Dtos.Products
{
    public class SpecialOfferDto
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductDto>Products { get; set; }
        public string Message { get; set; }
    }
}
