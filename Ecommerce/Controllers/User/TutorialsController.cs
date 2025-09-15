using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialsController : ControllerBase
    {
        private readonly Services.User.ServicesAndTutorials.ITutorialsService _tutorialsService;
        public TutorialsController(Services.User.ServicesAndTutorials.ITutorialsService tutorialsService)
        {
            _tutorialsService = tutorialsService;
        }
        [HttpGet("GetAllTutorials")]
        public async Task<IActionResult> GetAllTutorials()
        {
            var result = await _tutorialsService.GetAllTutorialsAsync();
            return Ok(result);
        }
        [HttpGet("GetTutorialById/{tutorialId}")]
        public async Task<IActionResult> GetTutorialById(int tutorialId)
        {
            var result = await _tutorialsService.GetTutorialByIdAsync(tutorialId);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetTutorialByName/{name}")]
        public async Task<IActionResult> GetTutorialByName(string name)
        {
            var result = await _tutorialsService.GetTutorialByNameAsync(name);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
