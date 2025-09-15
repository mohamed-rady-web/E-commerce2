using Ecommerce.Dtos.Products;
using Ecommerce.Models.Product;
using Ecommerce.Models.User;

namespace Ecommerce.Dtos.User.Reviews
{
    public class ReviewsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
       public string Email { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ProductId { get; set; }
        public string Productname { get; set; }
    }
}
