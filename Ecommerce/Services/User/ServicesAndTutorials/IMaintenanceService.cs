using Ecommerce.Dtos.User.TutorialsAndMaintenance;

namespace Ecommerce.Services.User.ServicesAndTutorials
{
    public interface IMaintenanceService
    {
        public Task<MaintenanceServiceDto> AddServiceAsync(AddMaintenanceDto dto);
        public Task<MaintenanceServiceDto> UpdateServiceAsync(int ServiceId,UpdateMaintenaceDto dto);
        public Task<MaintenanceServiceDto> DeleteServiceAsync(int serviceId);
        public Task<List<MaintenanceServiceDto>> GetAllServicesAsync();
        public Task<MaintenanceServiceDto> GetServiceByIdAsync(int serviceId);
        public Task<MaintenanceServiceDto> GetServiceByNameAsync(string name);
    }
}
