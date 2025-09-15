using Ecommerce.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CheckOutAdminController : ControllerBase
    {
        private readonly ICheckOutService _checkOutService;

        public CheckOutAdminController(ICheckOutService checkOutService)
        {
            _checkOutService = checkOutService;
        }
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _checkOutService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [HttpPut("ConfirmPayment/{CheckOutId}")]
        public async Task<IActionResult> ConfirmPayment(int CheckOutId, [FromQuery] string newstatus)
        {
            var order = await _checkOutService.ConfirmPaymentAsync(CheckOutId, newstatus);
            return Ok(order);
        }
    }
}
