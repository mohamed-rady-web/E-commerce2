using Ecommerce.Services.User.AboutAndContact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }
        [HttpGet("About")]
        public async Task<IActionResult> GetAbout()
        {
            var result=await _aboutService.GetAboutInfoAsync();
            return Ok(result);
        }
    }
}
    