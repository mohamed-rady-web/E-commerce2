using Ecommerce.Dtos.User.Fav;
using Ecommerce.Services.User.FavAndBooking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavService _favService;

        public FavoriteController(IFavService favService)
        {
            _favService = favService;
        }

        [HttpPost("add-to-fav")]
        public async Task<IActionResult> AddToFav([FromBody] AddtoFavDto dto)
        {
            var result = await _favService.AddToFavAsync(dto);
            if (result.Message.Contains("error"))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("FavoriteList")]
        public async Task<IActionResult> GetAllItems(FavDto dto)
        {
            var items = await _favService.GetAllFavItemsAsync(dto.UserId);
            if (items == null)
            {
                return BadRequest();
            }
            return Ok(items);
        }
    [HttpGet("SearchById/{id}")]
        public async Task<IActionResult> GetItemById(FavItemDto dto)
        {
            var userId = User.FindFirst("UserId").Value;
            var item = await _favService.GetFavItemByIdAsync(userId,dto.FavItemId);
            return Ok(item);
        }
        [HttpGet("SearchByName/{name}")]
        public async Task<IActionResult>GetItemByName(FavItemDto dto)
        {
            var userId = User.FindFirst("UserId").Value;
            var item = await _favService.GetFavItemByProductnameAsync(userId,dto.ProductName);
            return Ok(item);
        }
        [HttpDelete("DeleteItemById/{id}")]
        public async Task<IActionResult>DeleteItem(FavItemDto dto)
        {
            var userId = User.FindFirst("UserId").Value;
            var item=await _favService.DeleteFromFavAsync(userId,dto.FavItemId);
            return Ok();

        }
    }
}
