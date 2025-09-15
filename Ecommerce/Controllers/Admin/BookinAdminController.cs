using Ecommerce.Dtos.User.Reservation;
using Ecommerce.Services.User.FavAndBooking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="Admin")]
    public class BookinAdminController : ControllerBase
    {
        private readonly IBookService _bookingService;
        public BookinAdminController(IBookService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPut("updateBooking/{bookingId}")]
        public async Task<IActionResult> UpdateBooking(int bookingId, [FromBody] UpdateBookingDto dto)
        {
            var booking = await _bookingService.updateReservation(bookingId, dto);
            if (booking is not null)
            {
                return Ok(booking);
            }
            return NotFound(new { Message = "Booking not found" });
        }
        [HttpPost("addAvailableDates")]
        public async Task<IActionResult> AddAvailableDates([FromBody] AddAvailableDatesDto dto)
        {
            var availableDate = await _bookingService.AddServiceDate(dto);
            return Ok(availableDate);
        }

    }
}
