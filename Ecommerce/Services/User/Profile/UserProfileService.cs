using Ecommerce.Data;
using Ecommerce.Dtos.Cart;
using Ecommerce.Dtos.CheckOut;
using Ecommerce.Dtos.Orders;
using Ecommerce.Dtos.User.Fav;
using Ecommerce.Dtos.User.Profile;
using Ecommerce.Dtos.User.Reservation;
using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Models;
using Ecommerce.Models.User;
using Ecommerce.Services.User.Profile;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _context;

        public UserProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Cart)
                    .Include(u => u.Faveorites)
                    .Include(u => u.CheckOuts)
                    .Include(u => u.Orders)
                    .Include(u => u.Reviews)
                    .Include(u => u.Bookings)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return null;

                return new UserProfileDto
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    CartId = user.CartId,
                    Cart = user.Cart != null ? new CartDto { CartId = user.Cart.Id, UserId = user.Id } : null,
                    FavoriteId = user.FaveoriteId,
                    Favorite = user.Faveorites != null ? new FavDto { Id = user.Faveorites.Id, UserId = user.Id } : null,
                    CheckOutId = user.CheckOutId,
                    CheckOuts = user.CheckOuts?.Select(c => new CheckOutDto { Id = c.Id, UserId = user.Id }).ToList(),
                    Orders = user.Orders?.Select(o => new OrderDto { Id = o.Id, UserId = user.Id }).ToList(),
                    Reviews = user.Reviews?.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = r.Product?.Name
                    }).ToList(),
                    Bookings = user.Bookings?.Select(b => new BookingDto
                    {
                        Id = b.Id,
                        UserId = b.UserId,
                        BookingDate = b.BookingDate,
                        Status = b.Status
                    }).ToList(),
                    Message = "Profile retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user profile", ex);
            }
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(string userId, UpdateProfileDto dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) return null;

                var dtoProperties = typeof(UpdateProfileDto).GetProperties();
                var userProperties = typeof(ApplicationUser).GetProperties();

                foreach (var dtoProp in dtoProperties)
                {
                    var value = dtoProp.GetValue(dto);
                    if (value != null)
                    {
                        var userProp = userProperties.FirstOrDefault(p => p.Name == dtoProp.Name);
                        if (userProp != null)
                        {
                            userProp.SetValue(user, value);
                        }
                    }
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new UserProfileDto
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Message = "Profile updated successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating user profile", ex);
            }
        }


        public async Task<UserProfileDto> GetUserOrders(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Orders)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return null;

                return new UserProfileDto
                {
                    UserId = user.Id,
                    Orders = user.Orders?.Select(o => new OrderDto { Id = o.Id, UserId = user.Id }).ToList(),
                    Message = "Orders retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user orders", ex);
            }
        }

        public async Task<UserProfileDto> GetUserFavorites(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Faveorites)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return null;

                return new UserProfileDto
                {
                    UserId = user.Id,
                    FavoriteId = user.FaveoriteId,
                    Favorite = user.Faveorites != null ? new FavDto { Id = user.Faveorites.Id, UserId = user.Id } : null,
                    Message = "Favorites retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user favorites", ex);
            }
        }

        public async Task<UserProfileDto> GetUserBookings(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Bookings)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return null;

                return new UserProfileDto
                {
                    UserId = user.Id,
                    Bookings = user.Bookings?.Select(b => new BookingDto
                    {
                        Id = b.Id,
                        UserId = b.UserId,
                        BookingDate = b.BookingDate,
                        Status = b.Status
                    }).ToList(),
                    Message = "Bookings retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user bookings", ex);
            }
        }

        public async Task<UserProfileDto> GetUserReviews(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Reviews)
                    .ThenInclude(r => r.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return null;

                return new UserProfileDto
                {
                    UserId = user.Id,
                    Reviews = user.Reviews?.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = r.Product?.Name
                    }).ToList(),
                    Message = "Reviews retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting user reviews", ex);
            }
        }

        public async Task<UserProfileDto> DeleteUserProfileAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) return null;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return new UserProfileDto
                {
                    UserId = user.Id,
                    Message = "User profile deleted successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting user profile", ex);
            }
        }
    }
}
