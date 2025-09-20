using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.Fav;
using Ecommerce.Models.User.FavAndBooking;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.FavAndBooking
{
    public class FavService : IFavService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FavService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FavDto> AddToFavAsync(AddtoFavDto dto)
        {
            try
            {
                var existingItem = await _context.FaveoriteItems
                    .FirstOrDefaultAsync(m => m.ProductId == dto.ProductId);

                if (existingItem is not null)
                {
                    return new FavDto
                    {
                        Message = "Product is already in the Favorite List",
                    };
                }

                var item = _mapper.Map<FaveoriteItemModel>(dto);
                item.AddedDate = DateTime.Now;

                _context.FaveoriteItems.Add(item);
                await _context.SaveChangesAsync();

                return new FavDto
                {
                    Message = "Product added to favorites successfully"
                };
            }
            catch (Exception ex)
            {
                return new FavDto
                {
                    Message = $"An error occurred while adding to favorites: {ex.Message}"
                };
            }
        }

        public async Task<FavDto> DeleteFromFavAsync(string userId, int favItemId)
        {
            try
            {
                var item = await _context.FaveoriteItems
                    .Include(i => i.Favorite)
                    .FirstOrDefaultAsync(i => i.Id == favItemId && i.Favorite.UserId == userId);

                if (item is null)
                {
                    return new FavDto
                    {
                        UserId = userId,
                        Message = "Item not found in your favorites"
                    };
                }

                _context.FaveoriteItems.Remove(item);
                await _context.SaveChangesAsync();

                return new FavDto
                {
                    UserId = userId,
                    Message = "Item deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new FavDto
                {
                    UserId = userId,
                    Message = $"Error occurred: {ex.Message}"
                };
            }
        }

        public async Task<List<FavDto>> GetAllFavItemsAsync(string userId)
        {
            var fav = await _context.Faveorites
                .Include(f => f.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (fav is null)
                return new List<FavDto>();

            var result = _mapper.Map<FavDto>(fav);
            result.Message = "Favorites retrieved successfully";

            return new List<FavDto> { result };
        }

        public async Task<FavItemDto> GetFavItemByIdAsync(string userId, int favItemId)
        {
            try
            {
                var item = await _context.FaveoriteItems
                    .Include(i => i.Product)
                    .Include(i => i.Favorite)
                    .FirstOrDefaultAsync(i => i.Id == favItemId && i.Favorite.UserId == userId);

                return item is null ? null : _mapper.Map<FavItemDto>(item);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred: {ex.Message}");
            }
        }

        public async Task<FavItemDto> GetFavItemByProductnameAsync(string userId, string productName)
        {
            try
            {
                var existingItem = await _context.FaveoriteItems
                    .Include(fi => fi.Product)
                    .Include(fi => fi.Favorite)
                    .FirstOrDefaultAsync(fi =>
                        fi.Favorite.UserId == userId &&
                        fi.Product.Name.ToLower() == productName.ToLower());

                return existingItem is null ? null : _mapper.Map<FavItemDto>(existingItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving favorite item: {ex.Message}");
            }
        }
    }
}
