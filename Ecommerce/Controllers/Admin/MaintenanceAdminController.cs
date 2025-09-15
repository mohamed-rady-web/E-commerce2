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
    public class MaintenanceAdminController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceAdminController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }
    

    [HttpPost("AddService")]
        public async Task<IActionResult> AddService([FromBody] AddMaintenanceDto dto)
        {
            var result = await _maintenanceService.AddServiceAsync(dto);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("DeleteService/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var result = await _maintenanceService.DeleteServiceAsync(serviceId);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);

        }
        [HttpPut("UpdateService/{serviceId}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] UpdateMaintenaceDto dto)
        {
            var result = await _maintenanceService.UpdateServiceAsync(serviceId, dto);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
   

