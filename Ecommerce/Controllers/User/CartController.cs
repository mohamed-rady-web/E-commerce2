using Ecommerce.Dtos.Cart;
using Ecommerce.Services.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-item")]
        public async Task <IActionResult> AddItemToCart([FromBody]CartItemDto dto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId is null)
            {
                return Unauthorized(new { Message = "You should login to Addthe item" });
            }
            var cart = await _cartService.AddItemToCartAsync(userId, dto);
            return Ok(cart);
        }
        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId is null)
            {
                return Unauthorized(new { Message = "You should login to view the cart" });
            }
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }
        [HttpDelete("remove-item/{cartItemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartItemId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId is null)
            {
                return Unauthorized(new { Message = "You should login to remove the item" });
            }
            var cart = await _cartService.RemoveFromCartAsync(userId, cartItemId);
            return Ok(cart);
        }
    }
}
