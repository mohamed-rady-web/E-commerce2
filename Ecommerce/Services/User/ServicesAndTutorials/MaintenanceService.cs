using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Models.User.ServicesAndTutorials;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.ServicesAndTutorials
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MaintenanceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MaintenanceServiceDto> AddServiceAsync(AddMaintenanceDto dto)
        {
            try
            {
                var existingService = await _context.MaintenanceServices
                    .FirstOrDefaultAsync(s => s.ServiceName.ToLower() == dto.ServiceName.ToLower());

                if (existingService != null)
                {
                    return new MaintenanceServiceDto
                    {
                        Message = "A service with the same name already exists."
                    };
                }

                var newService = _mapper.Map<MaintenanceServicesModel>(dto);
                _context.MaintenanceServices.Add(newService);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<MaintenanceServiceDto>(newService);
                result.Message = "Service added successfully.";
                return result;
            }
            catch (Exception ex)
            {
                return new MaintenanceServiceDto
                {
                    Message = $"An error occurred while adding the service: {ex.Message}"
                };
            }
        }

        public async Task<MaintenanceServiceDto> DeleteServiceAsync(int serviceId)
        {
            try
            {
                var exitingService = await _context.MaintenanceServices.FirstOrDefaultAsync(s => s.Id == serviceId);
                if (exitingService is null)
                {
                    return new MaintenanceServiceDto { Message = "Service not found." };
                }

                _context.MaintenanceServices.Remove(exitingService);
                await _context.SaveChangesAsync();

                return new MaintenanceServiceDto { Message = "Service deleted successfully." };
            }
            catch (Exception ex)
            {
                return new MaintenanceServiceDto
                {
                    Message = $"An error occurred while deleting the service: {ex.Message}"
                };
            }
        }

        public async Task<List<MaintenanceServiceDto>> GetAllServicesAsync()
        {
            try
            {
                var services = await _context.MaintenanceServices
                    .Include(s => s.Tutorial)
                    .ToListAsync();

                return _mapper.Map<List<MaintenanceServiceDto>>(services);
            }
            catch
            {
                return new List<MaintenanceServiceDto>();
            }
        }

        public async Task<MaintenanceServiceDto> GetServiceByIdAsync(int serviceId)
        {
            try
            {
                var exitingService = await _context.MaintenanceServices
                    .Include(s => s.Tutorial)
                    .FirstOrDefaultAsync(s => s.Id == serviceId);

                if (exitingService is null)
                {
                    return new MaintenanceServiceDto { Message = "Service not found." };
                }

                var result = _mapper.Map<MaintenanceServiceDto>(exitingService);
                result.Message = "Service retrieved successfully.";
                return result;
            }
            catch (Exception ex)
            {
                return new MaintenanceServiceDto
                {
                    Message = $"An error occurred while retrieving the service: {ex.Message}"
                };
            }
        }

        public async Task<MaintenanceServiceDto> GetServiceByNameAsync(string name)
        {
            try
            {
                var exitingService = await _context.MaintenanceServices
                    .Include(s => s.Tutorial)
                    .FirstOrDefaultAsync(m => m.ServiceName.ToLower() == name.ToLower());

                if (exitingService is null)
                {
                    return new MaintenanceServiceDto { Message = "ServiceName not found" };
                }

                return _mapper.Map<MaintenanceServiceDto>(exitingService);
            }
            catch (Exception ex)
            {
                return new MaintenanceServiceDto
                {
                    Message = $"An error occurred while retrieving the service: {ex.Message}"
                };
            }
        }

        public async Task<MaintenanceServiceDto> UpdateServiceAsync(int ServiceId, UpdateMaintenaceDto dto)
        {
            try
            {
                var exitingService = await _context.MaintenanceServices
                    .Include(s => s.Tutorial)
                    .FirstOrDefaultAsync(i => i.Id == ServiceId);

                if (exitingService is null)
                {
                    return new MaintenanceServiceDto { Message = "Service Not Found" };
                }

                // Update only non-null fields
                _mapper.Map(dto, exitingService);

                await _context.SaveChangesAsync();

                var result = _mapper.Map<MaintenanceServiceDto>(exitingService);
                result.Message = "Service Updated Successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new MaintenanceServiceDto
                {
                    Message = $"An error occurred while updating the service: {ex.Message}"
                };
            }
        }
    }
}
