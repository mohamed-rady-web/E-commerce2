using Ecommerce.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckOutController : ControllerBase
    {
        private readonly ICheckOutService _checkOutService;

        public CheckOutController(ICheckOutService checkOutService)
        {
            _checkOutService = checkOutService;
        }
        [HttpPost("ProcessCheckOut")]
        public async Task<IActionResult> ProcessCheckOut([FromBody] Dtos.CheckOut.StartCheckOutDto dto)
        {
            var checkout = await _checkOutService.ProcessCheckOutAsync(dto);
            return Ok(checkout);
        }
        [HttpGet("GetCheckOutDetails/{CheckOutId}")]
        public async Task<IActionResult> GetCheckOutDetails(int CheckOutId)
        {
            var checkout = await _checkOutService.GetCheckOutDetailsAsync(CheckOutId);
            return Ok(checkout);
        }
        [HttpGet("GetUserCheckOutOrders/{userId}")]
        public async Task<IActionResult> GetUserCheckOutOrders(string userId)
        {
            var orders = await _checkOutService.GetUserCheckOutOrdersAsync(userId);
            return Ok(orders);
        }
        [HttpDelete("CancelOrder/{CheckOutId}")]
        public async Task<IActionResult> CancelOrder(int CheckOutId)
        {
            var order = await _checkOutService.CancelOrderAsync(CheckOutId);
            return Ok(order);
        }
    }
}
