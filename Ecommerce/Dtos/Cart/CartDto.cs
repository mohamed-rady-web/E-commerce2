using Ecommerce.Models.Cart;

namespace Ecommerce.Dtos.Cart
{
    public class CartDto
    {
        public string Message { get; set; }
        public int CartId { get; set; }
        public IList<CartItemDto> CartItems { get; set; }
        public string UserId { get; set; }

    }
}
