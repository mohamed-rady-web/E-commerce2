using Ecommerce.Data;
using Ecommerce.Dtos.Cart;
using Ecommerce.Models.Cart;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Ecommerce.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDto> AddItemToCartAsync(string userId, CartItemDto dto)
        {
            try
            {
                var cart = await _context.Carts
                   .Include(c => c.CartItems)
                   .ThenInclude(ci => ci.Product)
                   .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new CartModel
                    {
                        UserId = userId,
                        CartItems = new List<CartItemModel>()
                    };
                    _context.Carts.Add(cart);
                }

                var existingItem = cart.CartItems
                    .FirstOrDefault(ci => ci.ProductId == dto.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity += dto.Quantity;
                }
                else
                {
                    var newItem = _mapper.Map<CartItemModel>(dto);
                    cart.CartItems.Add(newItem);
                }

                await _context.SaveChangesAsync();

                var result = _mapper.Map<CartDto>(cart);
                result.Message = "Product added to cart successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new CartDto { Message = $"Error adding item to cart: {ex.Message}" };
            }
        }

        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            try
            {
                var cart = await _context.Carts
                   .Include(c => c.CartItems)
                   .ThenInclude(ci => ci.Product)
                   .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    return new CartDto
                    {
                        Message = "Cart is empty",
                        CartItems = new List<CartItemDto>()
                    };
                }

                var result = _mapper.Map<CartDto>(cart);
                result.Message = "Cart retrieved successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new CartDto { Message = $"Error retrieving cart: {ex.Message}" };
            }
        }

        public async Task<CartDto> RemoveFromCartAsync(string userId, int cartItemId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    return new CartDto { Message = "Cart not found" };
                }

                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem == null)
                {
                    return new CartDto { Message = "Product not found in cart" };
                }

                cart.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<CartDto>(cart);
                result.Message = "Product removed successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new CartDto { Message = $"Error removing product: {ex.Message}" };
            }
        }
    }
}
