namespace Ecommerce.Dtos.Products
{
    public class UpdateSpecialOfferDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool? IsActive { get; set; }
    }
}
