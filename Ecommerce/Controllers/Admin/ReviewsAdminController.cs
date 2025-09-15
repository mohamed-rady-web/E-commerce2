using Ecommerce.Services.User.AboutAndContact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReviewsAdminController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsAdminController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAllReviews()
        {
           
                var reviews = await _reviewService.GetAllReviewsAsync();
                return Ok(reviews);
            
        }
        [HttpGet("Getop3Reviews/{productId}")]
        public async Task<IActionResult> GetTop3Reviews(int productId)
        {
           
                var reviews = await _reviewService.GetTop3ReviewsAsync(productId);
                return Ok(reviews);

        }
        [HttpGet("AverageRating/{productId}")]
        public async Task<IActionResult> GetAverageRating(int productId)
        {
           
                var averageRating = await _reviewService.GetAverageRatingAsync(productId);
                return Ok(averageRating);

        }
    }
}
