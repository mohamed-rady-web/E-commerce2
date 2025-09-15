using Ecommerce.Dtos.Cart;

namespace Ecommerce.Services.Cart
{
    public interface ICartService
    {
        Task<CartDto> GetCartByUserIdAsync(string userId);
        Task<CartDto> AddItemToCartAsync(string userId, CartItemDto dto);
        Task<CartDto> RemoveFromCartAsync(string userId, int cartItemId);
    }
}
