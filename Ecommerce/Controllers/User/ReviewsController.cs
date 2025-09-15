using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Services.User.AboutAndContact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPut("UpdateReview/{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] UpdateReviewDto updateReviewDto)
        {

            var userId = User.FindFirst("UserId")?.Value;
            if (userId is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            var result = await _reviewService.UpdateReviewAsync(reviewId, userId, updateReviewDto);
            return Ok(result);

        }
        [HttpGet("GetUserReviews")]
        public async Task<IActionResult> GetUserReviews()
        {

            var userId = User.FindFirst("UserId")?.Value;
            if (userId is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            var reviews = await _reviewService.GetUserReviewsAsync(userId);
            return Ok(reviews);
        }
        [HttpDelete("DeleteReview/{reviewId}")]
        public async Task<IActionResult> DeleteUserReviews(int reviewId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            var result = await _reviewService.DeleteReviewAsync(reviewId, userId);
            if (!result)
            {
                return NotFound("Review not found or you are not authorized to delete this review.");
            }
            return NoContent();
        }
        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto addReviewDto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId is null)
            {
                return Unauthorized("User ID not found in token.");
            }
            addReviewDto.UserId = userId;
            var result = await _reviewService.AddReviewAsync(addReviewDto);
            return Ok(result);
        }
        [HttpGet("GetAllProductReviews/{productId}")]
        public async Task<IActionResult> GetAllProductReviews(int productId)
        {
            var reviews = await _reviewService.GetAllProductReviewsAsync(productId);
            return Ok(reviews);
        }
    }
}
