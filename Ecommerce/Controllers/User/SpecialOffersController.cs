using Ecommerce.Services.Product.InterFaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialOffersController : ControllerBase
    {
        private readonly ISpecialOffersService _specialOffersService;
        public SpecialOffersController(ISpecialOffersService specialOffersService)
        {
            _specialOffersService = specialOffersService;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveSpecialOffers()
        {
            var result = await _specialOffersService.GetActiveSpecialOffersAsync();
            if (result is null || !result.Any())
            {
                return NotFound("No active special offers found.");
            }
            return Ok(result);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSpecialOffers()
        {
            var result = await _specialOffersService.GetAllSpecialOffersAsync();
            if (result is null || !result.Any())
            {
                return NotFound("No special offers found.");
            }
            return Ok(result);
        }
        [HttpGet("getSpecialOffer-id/{id}")]
        public async Task<IActionResult> GetSpecialOfferById(int id)
        {
            var result = await _specialOffersService.GetSpecialOfferByIdAsync(id);
            if (result is null)
            {
                return NotFound("Special offer not found.");
            }
            return Ok(result);
        }
        [HttpGet("getSpecialOffer-name/{name}")]
        public async Task<IActionResult> GetSpecialOfferByName(string name)
        {
            var result = await _specialOffersService.GetSpecialOfferByNameAsync(name);
            if (result is null)
            {
                return NotFound("Special offer not found.");
            }
            return Ok(result);
        }
    }
}
