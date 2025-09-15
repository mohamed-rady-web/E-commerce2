using Ecommerce.Dtos.Orders;

namespace Ecommerce.Services.Order
{
    public interface IOrderService
    {
        public Task<OrderDto> CreateOrderAsync(string userId,CreateOrderDto dto);
        public Task<List<OrderDto>> GetAllOrdersAsync(string userId);
        public Task<OrderDto> UpdateOrderStatusAsync(string userId,int orderId,string status);
        public Task<OrderDto> DeleteOrderAsync(string userId,int orderId);
        public Task<OrderDto> GetOrderByIdAsync(string userId,int orderId);
        public Task<List<OrderDto>> GetOrdersByStatusAsync(string status,string userId);
        public Task<OrderDto>UpdateOrderAsync(string userId,int orderId,UpdateOrderDto dto);
        public Task <OrderDto> RemoveOrderItemAsync(string userId,int orderId,int orderItemId);

    }
}
