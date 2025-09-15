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
        public CheckOutService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CheckOutDto> CancelOrderAsync(int CheckOutId)
        {
            try
            {
                var exitingOrder = await _context.CheckOuts.FirstOrDefaultAsync(c => c.Id == CheckOutId);
                if (exitingOrder is null)
                {
                    return new CheckOutDto
                    {
                        Message = "the order isnt found or it didnt cheacked out"
                    };
                }
                exitingOrder.Orders.Clear();
                _context.CheckOuts.Remove(exitingOrder);
                await _context.SaveChangesAsync();
                return new CheckOutDto
                {
                    Message = "Order Deleted"
                };

            }
            catch (Exception ex)
            {
                return new CheckOutDto
                {
                    Message = ex.Message,
                };
            }
        }

        public async Task<CheckOutDto> ConfirmPaymentAsync(int CheckOutId, string newstatus)
        {
            try
            {
                var exitingOrder = await _context.CheckOuts.Include(c => c.Orders).FirstOrDefaultAsync(m => m.Id == CheckOutId);
                if (exitingOrder is null)
                {
                    return new CheckOutDto
                    {
                        Message = "order Not found or didnt checked out "
                    };
                }
                var order = await _context.Orders.Where(c=>c.CheckOutId==CheckOutId).FirstOrDefaultAsync(m => m.Id == CheckOutId);
                if (order is null) { return new CheckOutDto { Message = "Order not found " }; }
                if (order.Status == exitingOrder.Status) { return new CheckOutDto { Message = "Payment has confirmed already" }; }
                order.Status=newstatus;
                exitingOrder.Status=newstatus;
                await _context.SaveChangesAsync();
                return new CheckOutDto
                {
                    Id = exitingOrder.Id,
                    UserId = exitingOrder.UserId,
                    Message = "Payment Confirmed",
                    Status = newstatus

                };
            }
            catch (Exception ex)
            {
                return new CheckOutDto
                {
                    Message = ex.Message
                };
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

                return checkouts.Select(c => new CheckOutDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    FirstName = c.User?.FirstName,
                    LastName = c.User?.LastName,
                    ShippingAddress = c.ShippingAddress,
                    BillingAddress = c.BillingAddress,
                    PaymentMethod = c.PaymentMethod,
                    TotalAmount = c.TotalAmount,
                    CheckOutDate = c.CheckOutDate,
                    Orders = c.Orders.Select(o => new OrderDto
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        TotalPrice = o.TotalPrice,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        Items = o.Items.Select(i => new OrderItemDto
                        {
                            Id = i.Id,
                            Quantity = i.Quantity,
                            Price = i.Price,
                            OrderId = i.OrderId,
                            CartItemId = i.CartItemId
                        }).ToList()
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<CheckOutDto>
                {
                    new CheckOutDto
                    {
                        Message = $"Error: {ex.Message}"
                    }
                };
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
                    return new CheckOutDto
                    {
                        Message = $"Checkout with ID {checkOutId} not found"
                    };
                }

                return new CheckOutDto
                {
                    Id = checkout.Id,
                    UserId = checkout.UserId,
                    FirstName = checkout.User?.FirstName,
                    LastName = checkout.User?.LastName,
                    ShippingAddress = checkout.ShippingAddress,
                    BillingAddress = checkout.BillingAddress,
                    PaymentMethod = checkout.PaymentMethod,
                    TotalAmount = checkout.TotalAmount,
                    CheckOutDate = checkout.CheckOutDate,
                    Orders = checkout.Orders.Select(o => new OrderDto
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        TotalPrice = o.TotalPrice,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        Items = o.Items.Select(i => new OrderItemDto
                        {
                            Id = i.Id,
                            Quantity = i.Quantity,
                            Price = i.Price,
                            OrderId = i.OrderId,
                            CartItemId = i.CartItemId
                        }).ToList()
                    }).ToList(),
                    Message = "Checkout retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new CheckOutDto
                {
                    Message = $"Error retrieving checkout: {ex.Message}"
                };
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
                    return new List<CheckOutDto>
            {
                new CheckOutDto
                {
                    Message = $"No checkouts found for user {userId}"
                }
            };
                }

                return checkouts.Select(c => new CheckOutDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    FirstName = c.User?.FirstName,
                    LastName = c.User?.LastName,
                    ShippingAddress = c.ShippingAddress,
                    BillingAddress = c.BillingAddress,
                    PaymentMethod = c.PaymentMethod,
                    TotalAmount = c.TotalAmount,
                    CheckOutDate = c.CheckOutDate,
                    Orders = c.Orders.Select(o => new OrderDto
                    {
                        Id = o.Id,
                        UserId = o.UserId,
                        TotalPrice = o.TotalPrice,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        Items = o.Items.Select(i => new OrderItemDto
                        {
                            Id = i.Id,
                            Quantity = i.Quantity,
                            Price = i.Price,
                            OrderId = i.OrderId,
                            CartItemId = i.CartItemId
                        }).ToList()
                    }).ToList(),
                    Message = "Checkout retrieved successfully"
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<CheckOutDto>
        {
            new CheckOutDto
            {
                Message = $"Error retrieving user checkouts: {ex.Message}"
            }
        };
            }
        }


        public async Task<CheckOutDto> ProcessCheckOutAsync(StartCheckOutDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ Save customer info first
                var checkout = new CheckOutModel
                {
                    UserId = dto.UserId,
                    ShippingAddress = dto.ShippingAddress,
                    BillingAddress = dto.BillingAddress,
                    PaymentMethod = dto.PaymentMethod,
                    TotalAmount = dto.TotalAmount,
                    CheckOutDate = DateTime.UtcNow
                };

                await _context.CheckOuts.AddAsync(checkout);
                await _context.SaveChangesAsync();

                // ✅ Create savepoint here
                await transaction.CreateSavepointAsync("SaveCustomerInfo");

                try
                {
                    // 2️⃣ Create order
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

                    // 3️⃣ Cart items with stock check
                    var cartItems = await _context.CartItems
                        .Include(c => c.Product)
                        .Where(c => c.Cart.UserId == dto.UserId)
                        .ToListAsync();

                    foreach (var cartItem in cartItems)
                    {
                        if (cartItem.Product.Stock < cartItem.Quantity)
                            throw new InvalidOperationException(
                                $"Not enough stock for product {cartItem.Product.Name}");

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

                    // 4️⃣ Clear cart
                    _context.CartItems.RemoveRange(cartItems);
                    await _context.SaveChangesAsync();

                    // ✅ Commit if everything succeeded
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // ❌ Rollback order/items/stock, but keep checkout
                    await transaction.RollbackToSavepointAsync("SaveCustomerInfo");
                    await transaction.CommitAsync();

                    return new CheckOutDto
                    {
                        Id = checkout.Id,
                        UserId = checkout.UserId,
                        ShippingAddress = checkout.ShippingAddress,
                        BillingAddress = checkout.BillingAddress,
                        PaymentMethod = checkout.PaymentMethod,
                        TotalAmount = checkout.TotalAmount,
                        CheckOutDate = checkout.CheckOutDate,
                        Message = $"Checkout saved but order failed: {ex.Message}"
                    };
                }

                // ✅ Full success response
                return new CheckOutDto
                {
                    Id = checkout.Id,
                    UserId = checkout.UserId,
                    ShippingAddress = checkout.ShippingAddress,
                    BillingAddress = checkout.BillingAddress,
                    PaymentMethod = checkout.PaymentMethod,
                    TotalAmount = checkout.TotalAmount,
                    CheckOutDate = checkout.CheckOutDate,
                    Message = "Checkout and order completed successfully"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Checkout process failed completely: {ex.Message}");
            }
        }

    }
}
