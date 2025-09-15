using Ecommerce.Data;
using Ecommerce.Dtos.Cart;
using Ecommerce.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        } 
        public async Task<CartDto> AddItemToCartAsync(string userId, CartItemDto dto)
        {
            try
            {
                var cart = await _context.Carts
                   .Include(c => c.CartItems)
                   .FirstOrDefaultAsync(c => c.UserId == userId);

                var existingItem = cart.CartItems
                    .FirstOrDefault(ci => ci.ProductId == dto.ProductId);

                if (existingItem is not null)
                {
                    existingItem.Quantity += dto.Quantity;
                }
                else
                {

                    cart.CartItems.Add(new CartItemModel
                    {
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity
                    });
                }

                await _context.SaveChangesAsync();


                return new CartDto
                {
                    CartId = cart.Id,
                    Message = "Product added to cart successfully",
                    CartItems = cart.CartItems.Select(ci => new CartItemDto
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new CartDto
                {
                    Message = $"Error adding item to cart: {ex.Message}"
                }
                ;
            }
        }


        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.CartItems)
                   .FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart is null)
                {
                    return new CartDto
                    {
                        Message = "Cart is empty",
                        CartItems = new List<CartItemDto>()
                    };
                }
                return new CartDto
                {
                    CartId = cart.Id,
                    Message = "Cart retrieved successfully",
                    CartItems = cart.CartItems.Select(ci => new CartItemDto
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new CartDto
                {
                    Message = $"Error retrieving cart: {ex.Message}"
                };

            }
        }


        public async Task<CartDto> RemoveFromCartAsync(string userId, int cartItemId)
        {
            try
            {
                var cart= await _context.Carts.Include(cart => cart.CartItems).Where(cart=>cart.UserId==userId).SingleOrDefaultAsync();
                if(cart is null)
                {
                    return new CartDto
                    {
                        Message = "Cart Not Found"
                    };
                }
                var cartItem = await _context.CartItems.FirstOrDefaultAsync(t => t.Id == cartItemId);
                if (cartItem == null)
                    throw new KeyNotFoundException("Product not found in cart");

                cart.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                return new CartDto
                {
                    CartId = cart.Id,
                    Message = "Product removed successfully",
                    CartItems = cart.CartItems.Select(ci => new CartItemDto
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity
                    }).ToList()
                };

            }
            catch (Exception ex)
            {
                return new CartDto
                {
                    Message = $"Error retrieving cart: {ex.Message}"
                };

            }
        }
    }
}
