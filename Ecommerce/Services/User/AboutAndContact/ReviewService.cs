using Ecommerce.Data;
using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Models.Product;
using Ecommerce.Models.User;
using Ecommerce.Models.User.AboutAndContact;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.AboutAndContact
{
    public class ReviewsService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewsDto> AddReviewAsync(AddReviewDto dto)
        {
            try
            {
                var review = new ReviewsModel
                {
                    UserId = dto.UserId,
                    Rating = dto.Rating,
                    Comment = dto.Comment,
                    ReviewDate = dto.ReviewDate,
                    ProductId = dto.ProductId
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                var user = await _context.Users.FindAsync(dto.UserId);
                var product = await _context.Products.FindAsync(dto.ProductId);

                return new ReviewsDto
                {
                    Id = review.Id,
                    UserId = review.UserId,
                    Email = user?.Email,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewDate = review.ReviewDate,
                    ProductId = review.ProductId,
                    Productname = product?.Name
                };
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
                return await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .OrderByDescending(r => r.ReviewDate)
                    .Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = r.Product.Name
                    })
                    .ToListAsync();
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
                return await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .Where(r => r.ProductId == productId)
                    .OrderByDescending(r => r.ReviewDate)
                    .Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = r.Product.Name
                    })
                    .ToListAsync();
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
                return await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .Where(r => r.ProductId == productId)
                    .OrderByDescending(r => r.Rating)
                    .ThenByDescending(r => r.ReviewDate)
                    .Take(3)
                    .Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = r.Product.Name
                    })
                    .ToListAsync();
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
                return await _context.Reviews
                    .Include(r => r.User)
                    .Include(r => r.Product)
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.ReviewDate)
                    .Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = r.Product.Name
                    })
                    .ToListAsync();
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

                review.Rating = dto.Rating;
                review.Comment = dto.Comment;
                review.ReviewDate = DateTime.UtcNow;

                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();

                return new ReviewsDto
                {
                    Id = review.Id,
                    UserId = review.UserId,
                    Email = review.User.Email,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewDate = review.ReviewDate,
                    ProductId = review.ProductId,
                    Productname = review.Product.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating review: {ex.Message}");
            }
        }
    }
}
