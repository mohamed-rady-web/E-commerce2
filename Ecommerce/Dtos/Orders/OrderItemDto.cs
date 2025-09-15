using Ecommerce.Dtos.Cart;

namespace Ecommerce.Dtos.Orders
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int OrderId { get; set; }
        public int CartItemId { get; set; }
        public CartItemDto CartItem { get; set; }
    }
}
