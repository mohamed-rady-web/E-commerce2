using Ecommerce.Dtos.User.Reviews;

namespace Ecommerce.Services.User.AboutAndContact
{
    public interface IReviewService
    {
        public Task<ReviewsDto> AddReviewAsync(AddReviewDto dto);
        public Task<List<ReviewsDto>> GetAllReviewsAsync();
        public Task<List<ReviewsDto>> GetAllProductReviewsAsync(int productId);
        public Task<double> GetAverageRatingAsync(int productId);
        public Task<List<ReviewsDto>> GetTop3ReviewsAsync(int productId);
        public Task<List<ReviewsDto>> GetUserReviewsAsync(string userId);
        public Task<bool> DeleteReviewAsync(int reviewId, string userId);
        public Task<ReviewsDto> UpdateReviewAsync(int reviewId, string userId, UpdateReviewDto dto);
    }
}
