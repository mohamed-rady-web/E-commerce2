using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Services.User.ServicesAndTutorials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TutorialsAdminController : ControllerBase
    {
        private readonly ITutorialsService _tutorialsService;

        public TutorialsAdminController(ITutorialsService tutorialsService)
        {
            _tutorialsService = tutorialsService;
        }
        [HttpPost("CreateTutorial")]
        public async Task<IActionResult> CreateTutorial([FromBody] AddTutorialDto dto)
        {
            var result = await _tutorialsService.CreateTutorialAsync(dto);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("UpdateTutorial/{tutorialId}")]
        public async Task<IActionResult> UpdateTutorial(int tutorialId, [FromBody] UpdateTutorialDto dto)
        {
            var result = await _tutorialsService.UpdateTutorialAsync(tutorialId, dto);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("DeleteTutorial/{tutorialId}")]
        public async Task<IActionResult> DeleteTutorial(int tutorialId)
        {
            var result = await _tutorialsService.DeleteTutorialAsync(tutorialId);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
