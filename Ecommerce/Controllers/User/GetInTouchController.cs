using Ecommerce.Dtos.User.GetInTouch_About;
using Ecommerce.Services.User.AboutAndContact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetInTouchController : ControllerBase
    {
        private readonly IGetInTouchService _getInTouchService;
        public GetInTouchController(IGetInTouchService getInTouchService)
        {
            _getInTouchService = getInTouchService;
        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
           
                var result = await _getInTouchService.SendMessageAsync(dto);
                return Ok(result);
            
        }
        
    }
}
