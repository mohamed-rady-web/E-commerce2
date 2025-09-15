using Ecommerce.Models.Cart;
using Ecommerce.Models.User;

namespace Ecommerce.Models.Order
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<OrderItemModel> Items { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Pending";

        public int CheckOutId { get; set; }
        public CheckOutModel CheckOut { get; set; }
    }
}
