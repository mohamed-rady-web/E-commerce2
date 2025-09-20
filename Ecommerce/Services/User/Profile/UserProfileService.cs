using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.Profile;
using Ecommerce.Models.User;
using Ecommerce.Services.User.Profile;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserProfileService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                    .Include(u => u.Reviews).ThenInclude(r => r.Product)
                    .Include(u => u.Bookings)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) return null;

                var dto = _mapper.Map<UserProfileDto>(user);
                dto.Message = "Profile retrieved successfully";
                return dto;
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

                _mapper.Map(dto, user); // only non-null values will update

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<UserProfileDto>(user);
                result.Message = "Profile updated successfully";
                return result;
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

                var dto = _mapper.Map<UserProfileDto>(user);
                dto.Message = "Orders retrieved successfully";
                return dto;
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

                var dto = _mapper.Map<UserProfileDto>(user);
                dto.Message = "Favorites retrieved successfully";
                return dto;
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

                var dto = _mapper.Map<UserProfileDto>(user);
                dto.Message = "Bookings retrieved successfully";
                return dto;
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

                var dto = _mapper.Map<UserProfileDto>(user);
                dto.Message = "Reviews retrieved successfully";
                return dto;
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
