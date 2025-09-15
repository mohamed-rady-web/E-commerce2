using Ecommerce.Data;
using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Models.User.ServicesAndTutorials;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecommerce.Services.User.ServicesAndTutorials
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MaintenanceServiceDto> AddServiceAsync(AddMaintenanceDto dto)
        {
            try
            {
                var existingService = _context.MaintenanceServices.FirstOrDefaultAsync(s => s.ServiceName.ToLower() == dto.ServiceName.ToLower());
                if (existingService != null)
                {
                    return new MaintenanceServiceDto
                    {
                        Message = "A service with the same name already exists."
                    };
                }
                var newService = new MaintenanceServicesModel
                {
                    ServiceName = dto.ServiceName,
                    Description = dto.Description,
                    Price = dto.Price,
                    ImageUrl = dto.ImageUrl
                };
                _context.MaintenanceServices.Add(newService);
                await _context.SaveChangesAsync();
                return new MaintenanceServiceDto
                {
                    Id = newService.Id,
                    ServiceName = newService.ServiceName,
                    Description = newService.Description,
                    Price = newService.Price,
                    ImageUrl = newService.ImageUrl,
                    Message = "Service added successfully."
                };
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
                if(exitingService is null)
                {
                    return new MaintenanceServiceDto
                    {
                        Message = "Service not found."
                    };
                }
                _context.MaintenanceServices.Remove(exitingService);
                await _context.SaveChangesAsync();
                return new MaintenanceServiceDto {
                    Message = "Service deleted successfully."
                };

            }
            catch(Exception ex)
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
               var Services=await _context.MaintenanceServices.ToListAsync();
                return new List<MaintenanceServiceDto>(Services.Select(service => new MaintenanceServiceDto
                {
                    Message="Services retrieved successfully.",
                    Id = service.Id,
                    ServiceName = service.ServiceName,
                    Description = service.Description,
                    Price = service.Price,
                    ImageUrl = service.ImageUrl
                }));

            }
            catch(Exception ex)
            {
                return new List<MaintenanceServiceDto>();
            }
        }

        public async Task<MaintenanceServiceDto> GetServiceByIdAsync(int serviceId)
        {
            try
            {
                var exitingService=await _context.MaintenanceServices.FirstOrDefaultAsync(s=>s.Id==serviceId);
                if(exitingService is null)
                {
                    return new MaintenanceServiceDto
                    {
                        Message = "Service not found."
                    };
                }
                return new MaintenanceServiceDto
                {
                    Message = "Service retrieved successfully.",
                    Id = exitingService.Id,
                    ServiceName = exitingService.ServiceName,
                    Description = exitingService.Description,
                    Price = exitingService.Price,
                    ImageUrl = exitingService.ImageUrl
                };

            }
            catch(Exception ex)
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
                var exitingService = await _context.MaintenanceServices.FirstOrDefaultAsync(m => m.ServiceName.ToLower() == name);
                    if (exitingService is null)
                {
                    return new MaintenanceServiceDto
                    {
                        Message = "ServiceName not found"

                    };
                }
                return new MaintenanceServiceDto
                {
                    Id = exitingService.Id,
                    ServiceName = exitingService.ServiceName,
                    Description = exitingService.Description,
                    Price = exitingService.Price,
                    ImageUrl = exitingService.ImageUrl
                };


            } catch (Exception ex)
            {
                return new MaintenanceServiceDto
                {
                    Message = $"An error occurred while retrieving the service: {ex.Message}"
                };
            }
        }

        public async Task<MaintenanceServiceDto> UpdateServiceAsync(int ServiceId,UpdateMaintenaceDto dto)
        {
            try
            {
                var exitingService = await _context.MaintenanceServices.FirstOrDefaultAsync(i => i.Id == ServiceId);
                if (exitingService is null)
                {
                    return new MaintenanceServiceDto
                    {
                        Message = "Service Not Found"
                    };
                }

                var dtoProp = typeof(UpdateMaintenaceDto).GetProperties();
                var ModelProp = typeof(MaintenanceServicesModel).GetProperties();
                
                    foreach (var prop in dtoProp)
                    {
                        var value = prop.GetValue(dto);
                        if (value != null)
                        {
                            var entityProp = ModelProp.FirstOrDefault(p => p.Name == prop.Name);
                            entityProp?.SetValue(exitingService, value);
                        }
                    }

                
                await _context.SaveChangesAsync();
                return new MaintenanceServiceDto
                {
                    Id=exitingService.Id,
                    Message="Service Updated Successfully",
                    ServiceName=exitingService.ServiceName,
                    Description=exitingService.Description,
                    Price=exitingService.Price,
                    ImageUrl=exitingService.ImageUrl,
                    Tutorial=exitingService.Tutorial is not null ? new TutorialsDto
                    {
                        Id = exitingService.Tutorial.Id,
                        Title = exitingService.Tutorial.Title,
                        Description = exitingService.Tutorial.Description,
                        coverImageUrl = exitingService.Tutorial.coverImageUrl,
                        PublishedDate = exitingService.Tutorial.PublishedDate
                    } : null

                };
            }
            catch (Exception ex)
            {
                return new MaintenanceServiceDto
                {
                    Message = $" Message = $\"An error occurred while retrieving the service: {ex.Message}\""
                };
            }
        }
    }
}
