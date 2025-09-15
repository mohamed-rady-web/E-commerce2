using Ecommerce.Models.Order;
using Ecommerce.Models.Product;

namespace Ecommerce.Models.Cart
{
    public class CartItemModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CartId { get; set; }
        public CartModel Cart { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
        public int OrderItemId { get; set; }
        public OrderItemModel OrderItem { get; set; }
    }
}
