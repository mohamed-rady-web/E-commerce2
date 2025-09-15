using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceServicesController : ControllerBase
    {
        private readonly Services.User.ServicesAndTutorials.IMaintenanceService _maintenanceService;
        public MaintenanceServicesController(Services.User.ServicesAndTutorials.IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }
        [HttpGet("GetAllServices")]
        public async Task<IActionResult> GetAllServices()
        {
            var result = await _maintenanceService.GetAllServicesAsync();
            return Ok(result);
        }
        [HttpGet("GetServiceById/{serviceId}")]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var result = await _maintenanceService.GetServiceByIdAsync(serviceId);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetServiceByName/{name}")]
        public async Task<IActionResult> GetServiceByName(string name)
        {
            var result = await _maintenanceService.GetServiceByNameAsync(name);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
