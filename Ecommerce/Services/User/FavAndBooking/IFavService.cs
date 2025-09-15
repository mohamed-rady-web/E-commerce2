using Ecommerce.Dtos.User.Fav;

namespace Ecommerce.Services.User.FavAndBooking
{
    public interface IFavService
    {
        public Task<FavDto> AddToFavAsync(AddtoFavDto dto);
        public Task<FavDto> DeleteFromFavAsync(string userId, int favItemId);
        public Task<List<FavDto>> GetAllFavItemsAsync(string userId);
        public Task<FavItemDto> GetFavItemByIdAsync(string userId, int favItemId);
        public Task<FavItemDto> GetFavItemByProductnameAsync(string userId,string productName);
    }
}
