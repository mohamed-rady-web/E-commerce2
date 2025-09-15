using Ecommerce.Dtos.Products;
using Ecommerce.Services.Product.InterFaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SpecialOffersAdminController : ControllerBase
    {
        private readonly ISpecialOffersService _specialOffersService;
        public SpecialOffersAdminController(ISpecialOffersService specialOffersService)
        {
            _specialOffersService = specialOffersService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddSpecialOffer([FromBody] AddSpecialOfferDto addSpecialOfferDto)
        {
            var result = await _specialOffersService.CreateSpecialOfferAsync(addSpecialOfferDto);
            if (result is null)
            {
                return BadRequest("Failed to add special offer.");
            }
            return Ok(result);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSpecialOffer(int id, [FromBody] UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            var result = await _specialOffersService.UpdateSpecialOfferAsync(id, updateSpecialOfferDto);
            if (result is null)
            {
                return NotFound("Special offer not found.");
            }
            return Ok(result);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSpecialOffer(int id)
        {
            var success = await _specialOffersService.DeleteSpecialOfferAsync(id);
            if (success is null)
            {
                return NotFound("Special offer not found.");
            }
            return NoContent();
        }
        [HttpDelete("delete-inactive")]
        public async Task<IActionResult> DeleteInactiveSpecialOffers()
        {
            var result = await _specialOffersService.DeleteUnActiveSpecialOfferAsync();
            if (result is null)
            {
                return NotFound("No inactive special offers found.");
            }
            return Ok(result);
        }
    }
}
