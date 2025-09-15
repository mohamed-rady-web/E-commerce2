using Ecommerce.Dtos.User.Reservation;
using Ecommerce.Services.User.FavAndBooking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookService _bookingService;
        public BookingController(IBookService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost("Add-Reservation")]
        public async Task<IActionResult> AddReservation([FromBody] AddBookingDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId is null) return Unauthorized(new { Message = "You should hava an account to reserve" });
            var booking = await _bookingService.addReservation(dto);
            return Ok(booking);
        }
        [HttpGet("Get-All-Reservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId is null) return Unauthorized(new { Message = "You should hava an account to reserve" });
            var bookings = await _bookingService.getAllReservations(userId);
            return Ok(bookings);
        }
        [HttpGet("Get-Reservation-ById/{reservationId}")]
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId is null) return Unauthorized(new { Message = "You should hava an account to reserve" });
            var booking = await _bookingService.getReservationById(userId, reservationId);
            if (booking is not null)
            {
                return Ok(booking);
            }
            return NotFound(new { Message = "Booking not found" });
        }
        [HttpPut("Cancel-Reservation/{reservationId}")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId is null) return Unauthorized(new { Message = "You should hava an account to reserve" });
            var booking = await _bookingService.getReservationById(userId, reservationId);
            if (booking is null)
            {
                return NotFound(new { Message = "Booking not found" });
            }
            var canceledBooking = await _bookingService.CancelReservation(reservationId);
            return Ok(canceledBooking);
        }
        [HttpGet("Get-All-Reservation-Dates/{reservationId}")]
        public async Task<IActionResult> GetAllReservationDates(int reservationId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId is null) return Unauthorized(new { Message = "You should hava an account to reserve" });
            var booking = await _bookingService.getReservationById(userId, reservationId);
            if (booking is null)
            {
                return NotFound(new { Message = "Booking not found" });
            }
            var dates = await _bookingService.getAllReservationDates(reservationId);
            return Ok(dates);
        }
        [HttpPut("Update-Reservation/{reservationId}")]
        public async Task<IActionResult> UpdateReservation(int reservationId, [FromBody] UpdateBookingDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId is null) return Unauthorized(new { Message = "You should hava an account to reserve" });
            var booking = await _bookingService.getReservationById(userId, reservationId);
            if (booking is null)
            {
                return NotFound(new { Message = "Booking not found" });
            }
            var updatedBooking = await _bookingService.updateReservation(reservationId, dto);
            return Ok(updatedBooking);
        }
        [HttpGet("Get-Reservation-ByName/{firstName}")]
        public async Task<IActionResult> GetReservationByName(string firstName)
        {
            var bookings = await _bookingService.getReservationByName(firstName);
            if (bookings is not null && bookings.Count > 0)
            {
                return Ok(bookings);
            }
            return NotFound(new { Message = "No bookings found for the given name" });
        }




    }
}
