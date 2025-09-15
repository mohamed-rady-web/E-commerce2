using Ecommerce.Dtos.User.Profile;

namespace Ecommerce.Services.User.Profile
{
    public interface IUserProfileService
    {
        public Task<UserProfileDto> GetUserProfileAsync(string userId);
        public Task<UserProfileDto> UpdateUserProfileAsync(string userId, UpdateProfileDto dto);
        public Task<UserProfileDto> GetUserOrders(string userId);
        public Task<UserProfileDto> GetUserFavorites(string userId);
        public Task<UserProfileDto> GetUserBookings(string userId);
        public Task<UserProfileDto> GetUserReviews(string userId);
        public Task<UserProfileDto> DeleteUserProfileAsync(string userId);
    }
}
