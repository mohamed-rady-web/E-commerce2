using Ecommerce.Dtos.Orders;
using Ecommerce.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var order = await _orderService.CreateOrderAsync(userId, dto);
            return Ok(order);
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var orders = await _orderService.GetAllOrdersAsync(userId);
            return Ok(orders);
        }
        [HttpGet("get-byId/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var order = await _orderService.GetOrderByIdAsync(userId, orderId);
            return Ok(order);
        }
        [HttpGet("get-byStatus/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(string status)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var orders = await _orderService.GetOrdersByStatusAsync(status, userId);
            return Ok(orders);
        }
        [HttpDelete("delete-order/{id}")]
        public async Task<IActionResult> DeleteOrderById(int id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var order = await _orderService.DeleteOrderAsync(userId, id);
            return Ok(order);
        }
        [HttpDelete("remove-orderItem/{orderId}/{orderItemId}")]
        public async Task<IActionResult> RemoveOrderItem(int orderId, int orderItemId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var order = await _orderService.RemoveOrderItemAsync(userId, orderId, orderItemId);
            return Ok(order);
        }
        [HttpPut("update-order-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var order = await _orderService.UpdateOrderStatusAsync(userId, orderId, status);
            return Ok(order);
        }
        [HttpPut("update-order/{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromBody]UpdateOrderDto dto,int orderId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var order = await _orderService.UpdateOrderAsync(userId, orderId, dto);
            return Ok(order);
        }
    }
}
