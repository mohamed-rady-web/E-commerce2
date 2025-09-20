using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.CheckOut;
using Ecommerce.Dtos.Orders;
using Ecommerce.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Order
{
    public class CheckOutService : ICheckOutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CheckOutService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CheckOutDto> CancelOrderAsync(int CheckOutId)
        {
            try
            {
                var exitingOrder = await _context.CheckOuts.FirstOrDefaultAsync(c => c.Id == CheckOutId);
                if (exitingOrder is null)
                {
                    return new CheckOutDto { Message = "the order isnt found or it didnt cheacked out" };
                }

                exitingOrder.Orders.Clear();
                _context.CheckOuts.Remove(exitingOrder);
                await _context.SaveChangesAsync();

                return new CheckOutDto { Message = "Order Deleted" };
            }
            catch (Exception ex)
            {
                return new CheckOutDto { Message = ex.Message };
            }
        }

        public async Task<CheckOutDto> ConfirmPaymentAsync(int CheckOutId, string newstatus)
        {
            try
            {
                var exitingOrder = await _context.CheckOuts
                    .Include(c => c.Orders)
                    .FirstOrDefaultAsync(m => m.Id == CheckOutId);

                if (exitingOrder is null)
                {
                    return new CheckOutDto { Message = "order Not found or didnt checked out " };
                }

                var order = await _context.Orders
                    .Where(c => c.CheckOutId == CheckOutId)
                    .FirstOrDefaultAsync(m => m.Id == CheckOutId);

                if (order is null) return new CheckOutDto { Message = "Order not found " };
                if (order.Status == exitingOrder.Status) return new CheckOutDto { Message = "Payment has confirmed already" };

                order.Status = newstatus;
                exitingOrder.Status = newstatus;

                await _context.SaveChangesAsync();

                var dto = _mapper.Map<CheckOutDto>(exitingOrder);
                dto.Message = "Payment Confirmed";
                dto.Status = newstatus;

                return dto;
            }
            catch (Exception ex)
            {
                return new CheckOutDto { Message = ex.Message };
            }
        }

        public async Task<List<CheckOutDto>> GetAllOrdersAsync()
        {
            try
            {
                var checkouts = await _context.CheckOuts
                    .Include(c => c.User)
                    .Include(c => c.Orders)
                        .ThenInclude(o => o.Items)
                    .ToListAsync();

                return _mapper.Map<List<CheckOutDto>>(checkouts);
            }
            catch (Exception ex)
            {
                return new List<CheckOutDto> { new CheckOutDto { Message = $"Error: {ex.Message}" } };
            }
        }

        public async Task<CheckOutDto> GetCheckOutDetailsAsync(int checkOutId)
        {
            try
            {
                var checkout = await _context.CheckOuts
                    .Include(c => c.User)
                    .Include(c => c.Orders)
                        .ThenInclude(o => o.Items)
                    .FirstOrDefaultAsync(c => c.Id == checkOutId);

                if (checkout == null)
                {
                    return new CheckOutDto { Message = $"Checkout with ID {checkOutId} not found" };
                }

                var dto = _mapper.Map<CheckOutDto>(checkout);
                dto.Message = "Checkout retrieved successfully";
                return dto;
            }
            catch (Exception ex)
            {
                return new CheckOutDto { Message = $"Error retrieving checkout: {ex.Message}" };
            }
        }

        public async Task<List<CheckOutDto>> GetUserCheckOutOrdersAsync(string userId)
        {
            try
            {
                var checkouts = await _context.CheckOuts
                    .Include(c => c.User)
                    .Include(c => c.Orders)
                        .ThenInclude(o => o.Items)
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                if (!checkouts.Any())
                {
                    return new List<CheckOutDto> { new CheckOutDto { Message = $"No checkouts found for user {userId}" } };
                }

                var result = _mapper.Map<List<CheckOutDto>>(checkouts);
                result.ForEach(c => c.Message = "Checkout retrieved successfully");

                return result;
            }
            catch (Exception ex)
            {
                return new List<CheckOutDto> { new CheckOutDto { Message = $"Error retrieving user checkouts: {ex.Message}" } };
            }
        }

        public async Task<CheckOutDto> ProcessCheckOutAsync(StartCheckOutDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var checkout = _mapper.Map<CheckOutModel>(dto);
                checkout.CheckOutDate = DateTime.UtcNow;

                await _context.CheckOuts.AddAsync(checkout);
                await _context.SaveChangesAsync();

                await transaction.CreateSavepointAsync("SaveCustomerInfo");

                try
                {
                    var order = new OrderModel
                    {
                        UserId = dto.UserId,
                        CheckOutId = checkout.Id,
                        OrderDate = DateTime.UtcNow,
                        Status = "Pending",
                        TotalPrice = dto.TotalAmount
                    };

                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    var cartItems = await _context.CartItems
                        .Include(c => c.Product)
                        .Where(c => c.Cart.UserId == dto.UserId)
                        .ToListAsync();

                    foreach (var cartItem in cartItems)
                    {
                        if (cartItem.Product.Stock < cartItem.Quantity)
                            throw new InvalidOperationException($"Not enough stock for product {cartItem.Product.Name}");

                        cartItem.Product.Stock -= cartItem.Quantity;

                        var orderItem = new OrderItemModel
                        {
                            OrderId = order.Id,
                            CartItemId = cartItem.Id,
                            Quantity = cartItem.Quantity,
                            Price = cartItem.Price
                        };

                        await _context.OrderItems.AddAsync(orderItem);
                    }

                    await _context.SaveChangesAsync();

                    _context.CartItems.RemoveRange(cartItems);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackToSavepointAsync("SaveCustomerInfo");
                    await transaction.CommitAsync();

                    var partial = _mapper.Map<CheckOutDto>(checkout);
                    partial.Message = $"Checkout saved but order failed: {ex.Message}";

                    return partial;
                }

                var result = _mapper.Map<CheckOutDto>(checkout);
                result.Message = "Checkout and order completed successfully";
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Checkout process failed completely: {ex.Message}");
            }
        }
    }
}
