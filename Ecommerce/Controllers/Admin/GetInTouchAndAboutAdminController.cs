using Ecommerce.Services.User.AboutAndContact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class GetInTouchAndAboutAdminController : ControllerBase
    {
        private readonly IGetInTouchService _getInTouchService;

        public GetInTouchAndAboutAdminController(IGetInTouchService getInTouchService)
        {
            _getInTouchService = getInTouchService;
        }
        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _getInTouchService.GetAllMessagesAsync();
            return Ok(messages);
        }
        [HttpGet("Message-By-Id/{messageId}")]
        public async Task<IActionResult> GetMessageById(int messageId)
        {
            var message = await _getInTouchService.GetMessageByIdAsync(messageId);
            if (message == null)
            {
                return NotFound($"Message with ID {messageId} not found.");
            }
            return Ok(message);
        }
        [HttpDelete("DeleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var result = await _getInTouchService.DeleteMessageAsync(messageId);
            if (result is null )
            {
                return NotFound($"Message with ID {messageId} not found.");
            }
            return Ok(result);
        }
    }
}
