using Ecommerce.Dtos.User.Profile;
using Ecommerce.Services.User.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        [HttpGet("Get-profile/{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var userIdFromToken = User.FindFirst("UserId")?.Value;
            if (userIdFromToken is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            if (userIdFromToken != userId)
            {
                return Forbid("You are not authorized to access this profile.");
            }
            var profile = await _userProfileService.GetUserProfileAsync(userId);
            if (profile is null)
            {
                return NotFound($"Profile for User ID {userId} not found.");
            }
            return Ok(profile);
        }
        [HttpPut("Update-profile/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(string userId, [FromBody] UpdateProfileDto updateUserProfileDto)
        {
            var userIdFromToken = User.FindFirst("UserId")?.Value;
            if (userIdFromToken is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            if (userIdFromToken != userId)
            {
                return Forbid("You are not authorized to update this profile.");
            }
            var updatedProfile = await _userProfileService.UpdateUserProfileAsync(userId, updateUserProfileDto);
            if (updatedProfile is null)
            {
                return NotFound($"Profile for User ID {userId} not found.");
            }
            return Ok(updatedProfile);
        }
        [HttpDelete("Delete-profile/{userId}")]
        public async Task<IActionResult> DeleteUserProfile(string userId)
        {
            var userIdFromToken = User.FindFirst("UserId")?.Value;
            if (userIdFromToken is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            if (userIdFromToken != userId)
            {
                return Forbid("You are not authorized to delete this profile.");
            }
            var result = await _userProfileService.DeleteUserProfileAsync(userId);
            if (result is null)
            {
                return NotFound($"Profile for User ID {userId} not found.");
            }
            return Ok(result);
        }
        [HttpGet("GetUserOrders/{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            var userIdFromToken = User.FindFirst("UserId")?.Value;
            if (userIdFromToken is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            if (userIdFromToken != userId)
            {
                return Forbid("You are not authorized to access these orders.");
            }
            var orders = await _userProfileService.GetUserOrders(userId);
            return Ok(orders);
        }
        [HttpGet("GetUserBooking/{userId}")]
        public async Task<IActionResult> GetUserBooking(string userId)
        {
            var userIdFromToken = User.FindFirst("UserId")?.Value;
            if (userIdFromToken is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            if (userIdFromToken != userId)
            {
                return Forbid("You are not authorized to access these bookings.");
            }
            var bookings = await _userProfileService.GetUserBookings(userId);
            return Ok(bookings);
        }
        [HttpGet("GetUserFavorites/{userId}")]
        public async Task<IActionResult> GetUserFavorites(string userId)
        {
            var userIdFromToken = User.FindFirst("UserId")?.Value;
            if (userIdFromToken is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            if (userIdFromToken != userId)
            {
                return Forbid("You are not authorized to access these favorites.");
            }
            var favorites = await _userProfileService.GetUserFavorites(userId);
            return Ok(favorites);
        }
        [HttpGet("GetUserReviews/{userId}")]
        public async Task<IActionResult>GetUserReviews(string userId)
        {
            var userIdFromToken = User.FindFirst("UserId")?.Value;
            if (userIdFromToken is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            if (userIdFromToken != userId)
            {
                return Forbid("You are not authorized to access these favorites.");
            }
            var Reviews= await _userProfileService.GetUserReviews(userId);
            return Ok(Reviews);

        }
    }
}
