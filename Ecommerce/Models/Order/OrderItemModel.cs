using Ecommerce.Models.Cart;

namespace Ecommerce.Models.Order
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }  

        public int OrderId { get; set; }
        public OrderModel Order { get; set; }

        public int CartItemId { get; set; }   
        public CartItemModel CartItem { get; set; }
    }
    }

