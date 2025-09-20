using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Models.User;
using Ecommerce.Models.User.AboutAndContact;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.AboutAndContact
{
    public class ReviewsService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReviewsDto> AddReviewAsync(AddReviewDto dto)
        {
            try
            {
                var review = _mapper.Map<ReviewsModel>(dto);

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // fetch related entities for Email & Productname
                var reviewWithIncludes = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .FirstOrDefaultAsync(r => r.Id == review.Id);

                return _mapper.Map<ReviewsDto>(reviewWithIncludes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while adding review: {ex.Message}");
            }
        }

        public async Task<List<ReviewsDto>> GetAllReviewsAsync()
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToListAsync();

                return _mapper.Map<List<ReviewsDto>>(reviews);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving reviews: {ex.Message}");
            }
        }

        public async Task<List<ReviewsDto>> GetAllProductReviewsAsync(int productId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .Where(r => r.ProductId == productId)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToListAsync();

                return _mapper.Map<List<ReviewsDto>>(reviews);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving product reviews: {ex.Message}");
            }
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            try
            {
                return await _context.Reviews.AnyAsync(r => r.ProductId == productId)
                    ? await _context.Reviews.Where(r => r.ProductId == productId).AverageAsync(r => r.Rating)
                    : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while calculating average rating: {ex.Message}");
            }
        }

        public async Task<List<ReviewsDto>> GetTop3ReviewsAsync(int productId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .Where(r => r.ProductId == productId)
                    .OrderByDescending(r => r.Rating)
                    .ThenByDescending(r => r.ReviewDate)
                    .Take(3)
                    .ToListAsync();

                return _mapper.Map<List<ReviewsDto>>(reviews);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving top reviews: {ex.Message}");
            }
        }

        public async Task<List<ReviewsDto>> GetUserReviewsAsync(string userId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToListAsync();

                return _mapper.Map<List<ReviewsDto>>(reviews);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving user reviews: {ex.Message}");
            }
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, string userId)
        {
            try
            {
                var review = await _context.Reviews
                    .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

                if (review == null)
                    return false;

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting review: {ex.Message}");
            }
        }

        public async Task<ReviewsDto> UpdateReviewAsync(int reviewId, string userId, UpdateReviewDto dto)
        {
            try
            {
                var review = await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

                if (review == null)
                    throw new Exception("Review not found or not authorized.");

                _mapper.Map(dto, review);
                review.ReviewDate = DateTime.UtcNow;

                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();

                return _mapper.Map<ReviewsDto>(review);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating review: {ex.Message}");
            }
        }
    }
}
