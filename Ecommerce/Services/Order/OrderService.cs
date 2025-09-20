using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.Cart;
using Ecommerce.Dtos.Orders;
using Ecommerce.Models.Order;
using Ecommerce.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public OrderService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            try
            {
                var order = _mapper.Map<OrderModel>(dto);
                order.UserId = userId;
                order.OrderDate = DateTime.UtcNow;
                order.Status = "Pending";
                order.TotalPrice = order.Items.Sum(i => i.Quantity * i.Price);

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.Message = "Order created successfully";

                return orderDto;
            }
            catch (Exception ex)
            {
                return new OrderDto { Message = ex.Message };
            }
        }

        public async Task<OrderDto> DeleteOrderAsync(string userId, int orderId)
        {
            try
            {
                var exitingOrder = await _context.Orders
                    .Where(u => u.UserId == userId)
                    .SingleOrDefaultAsync(o => o.Id == orderId);

                if (exitingOrder is null)
                    return new OrderDto { Message = "Order not found" };

                if (exitingOrder.Status == "Deliverd" || exitingOrder.Status == "Canceled" || exitingOrder.Id == orderId)
                {
                    _context.Orders.Remove(exitingOrder);
                    await _context.SaveChangesAsync();
                }

                return new OrderDto { Message = "Order deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new OrderDto { Message = ex.Message };
            }
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

                IQueryable<OrderModel> query;

                if (isInRole)
                {
                    query = _context.Orders
                        .Include(o => o.User)
                        .Include(o => o.Items)
                            .ThenInclude(c => c.CartItem)
                        .OrderBy(o => o.OrderDate);
                }
                else
                {
                    query = _context.Orders
                        .Where(u => u.UserId == userId)
                        .Include(o => o.Items)
                            .ThenInclude(c => c.CartItem);
                }

                var orders = await query.ToListAsync();
                var orderDtos = _mapper.Map<List<OrderDto>>(orders);
                orderDtos.ForEach(o => o.Message = "Orders retrieved successfully");

                return orderDtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the orders: {ex.Message}");
            }
        }

        public async Task<OrderDto> GetOrderByIdAsync(string userId, int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Where(u => u.UserId == userId)
                    .Include(o => o.Items)
                    .SingleOrDefaultAsync(o => o.Id == orderId);

                if (order is null)
                {
                    return new OrderDto { Message = "you are not authorized to delete this order or order not found by this ID" };
                }

                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                return new OrderDto { Message = ex.Message };
            }
        }

        public async Task<List<OrderDto>> GetOrdersByStatusAsync(string status, string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

                if (!isInRole)
                    throw new Exception("You are not authorized");

                var allOrders = await _context.Orders
                    .Where(s => s.Status == status)
                    .Include(u => u.User)
                    .Include(o => o.Items)
                        .ThenInclude(c => c.CartItem)
                    .ToListAsync();

                var dtos = _mapper.Map<List<OrderDto>>(allOrders);
                dtos.ForEach(o => o.Message = "Orders retrieved successfully");

                return dtos;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the orders: {ex.Message}");
            }
        }

        public async Task<OrderDto> UpdateOrderAsync(string userId, int orderId, UpdateOrderDto dto)
        {
            try
            {
                var order = await _context.Orders
                    .Where(o => o.UserId == userId && o.Id == orderId)
                    .Include(o => o.Items)
                    .SingleOrDefaultAsync();

                if (order == null)
                {
                    return new OrderDto { Message = "Order not found or you do not have permission." };
                }

                order.Status = dto.Status ?? order.Status;
                order.TotalPrice = dto.TotalPrice > 0 ? dto.TotalPrice : order.Items.Sum(i => i.Price * i.Quantity);

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                var dtoResult = _mapper.Map<OrderDto>(order);
                dtoResult.Message = "Order updated successfully";

                return dtoResult;
            }
            catch (Exception ex)
            {
                return new OrderDto { Message = $"Error updating order: {ex.Message}" };
            }
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(string userId, int orderId, string status)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
                if (!isInRole)
                {
                    return new OrderDto { Message = "Not authorized" };
                }

                var order = await _context.Orders
                    .Include(u => u.User)
                    .Include(o => o.Items)
                        .ThenInclude(c => c.CartItem)
                    .SingleOrDefaultAsync(o => o.Id == orderId);

                if (order is null)
                {
                    return new OrderDto { Message = "an error happend when searching on the order" };
                }

                order.Status = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                var dto = _mapper.Map<OrderDto>(order);
                dto.Message = "Order status updated";

                return dto;
            }
            catch (Exception ex)
            {
                return new OrderDto { Message = $"Error Happend {ex.Message}" };
            }
        }

        public async Task<OrderDto> RemoveOrderItemAsync(string userId, int orderId, int orderItemId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .SingleOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return new OrderDto { Message = "Order not found or you do not have permission." };

            var item = order.Items.SingleOrDefault(i => i.Id == orderItemId);
            if (item == null)
                return new OrderDto { Message = "Order item not found." };

            order.Items.Remove(item);
            _context.OrderItems.Remove(item);

            order.TotalPrice = order.Items.Sum(i => i.Price * i.Quantity);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<OrderDto>(order);
            dto.Message = "Order item removed successfully";

            return dto;
        }
    }
}
