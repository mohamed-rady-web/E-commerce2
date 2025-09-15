using Ecommerce.Models.Product;

namespace Ecommerce.Models.User.AboutAndContact
{
    public class ReviewsModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}
