using Ecommerce.Data;
using Ecommerce.Dtos.User.Fav;
using Ecommerce.Models.User.FavAndBooking;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.FavAndBooking
{
    public class FavService : IFavService
    {
        private readonly ApplicationDbContext _context;

        public FavService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FavDto> AddToFavAsync(AddtoFavDto dto)
        {
            try
            {
                var exitingitem = await _context.FaveoriteItems.SingleOrDefaultAsync(m => m.ProductId == dto.ProductId);
                if (exitingitem is not null)
                {
                    return new FavDto
                    {
                        Message = "Product is already in the FavioriteList",

                    };
                }
                var item = new FaveoriteItemModel
                {
                    ProductId = dto.ProductId,
                    AddedDate = DateTime.Now,
                };
                _context.FaveoriteItems.Add(item);
                await _context.SaveChangesAsync();
                return new FavDto
                {
                    Message = "Product Add To Fav Successfully"
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

            return new List<FavDto>
    {
        new FavDto
        {
            UserId = fav.UserId,
            Items = fav.Items.Select(i => new FavItemDto
            {
                FavItemId = i.Id,
                ProductId = i.Product.Id,
                ProductName = i.Product.Name,
                ImageUrl = i.Product.ImageUrl,
                Price = i.Product.Price
            }).ToList(),
            Message = "Favorites retrieved successfully"
        }
    };
        }



        public async Task<FavItemDto> GetFavItemByIdAsync(string userId, int favItemId)
        {
            try
            {
                var item = await _context.FaveoriteItems
                    .Include(i => i.Product)
                    .Include(i => i.Favorite)
                    .FirstOrDefaultAsync(i => i.Id == favItemId && i.Favorite.UserId == userId);

                if (item is null)
                    return null;

                return new FavItemDto
                {
                    FavItemId = item.Id,
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    ImageUrl = item.Product.ImageUrl,
                    Price = item.Product.Price
                };
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

                if (existingItem is null)
                    return null;

                return new FavItemDto
                {
                    FavItemId=existingItem.Id,
                    ProductId = existingItem.Product.Id,
                    ProductName = existingItem.Product.Name,
                    ImageUrl = existingItem.Product.ImageUrl,
                    Price = existingItem.Product.Price
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving favorite item: {ex.Message}");
            }
        }

    }
}

