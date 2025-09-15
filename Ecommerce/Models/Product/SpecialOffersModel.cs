namespace Ecommerce.Models.Product
{
    public class SpecialOffersModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ProductModel> Products { get; set; }
        public bool IsActive { get; set; }
    }
}
