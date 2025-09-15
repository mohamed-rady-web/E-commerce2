using Ecommerce.Data;
using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Models.User.ServicesAndTutorials;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.ServicesAndTutorials
{
    public class TutorialsService : ITutorialsService
    {
        private readonly ApplicationDbContext _context;

        public TutorialsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TutorialsDto> CreateTutorialAsync(AddTutorialDto dto)
        {
            try
            {
                var exitingTutorial = await _context.Tutorials.FirstOrDefaultAsync(t => t.Title.ToLower() == dto.Title.ToLower());
                if (exitingTutorial is null)
                {
                    return new TutorialsDto
                    {
                        Message = "Tutorial with the same title already exists."
                    };
                }
                var newTutorial = new TutorialsModel
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    coverImageUrl = dto.coverImageUrl,
                    PublishedDate = DateTime.UtcNow
                };
                _context.Tutorials.Add(newTutorial);
                await _context.SaveChangesAsync();
                return new TutorialsDto
                {
                    Id = newTutorial.Id,
                    Title = newTutorial.Title,
                    Description = newTutorial.Description,
                    coverImageUrl = newTutorial.coverImageUrl,
                    PublishedDate = newTutorial.PublishedDate,
                    Message = "Tutorial created successfully."
                };
            }
            catch (Exception ex)
            {
                return new TutorialsDto
                {
                    Message = $"An error occurred while checking for existing tutorial: {ex.Message}"
                };

            }
        }

        public async Task<TutorialsDto> DeleteTutorialAsync(int tutorialId)
        {
            try
            {
                var exitingTutorial = await _context.Tutorials.FirstOrDefaultAsync(t => t.Id == tutorialId);
                if(exitingTutorial is null)
                {
                    return new TutorialsDto
                    {
                        Message = "Tutorial not found."
                    };
                }
                _context.Tutorials.Remove(exitingTutorial);
                await _context.SaveChangesAsync();
                return new TutorialsDto {
                    Message = "Tutorial deleted successfully."
                };
            }catch(Exception ex)
            {
                return new TutorialsDto
                {
                    Message = $"An error occurred while deleting the tutorial: {ex.Message}"
                };
            }
        }

        public async Task<List<TutorialsDto>> GetAllTutorialsAsync()
        {
            try
            {
                var Tutorials = await _context.Tutorials.Include(m => m.RelatedService).ToListAsync();

                return new List<TutorialsDto>(Tutorials.Select(Tutorials => new TutorialsDto
                {
                    Id = Tutorials.Id,
                    Title=Tutorials.Title,
                    PublishedDate = Tutorials.PublishedDate,
                    Description=Tutorials.Description,
                    coverImageUrl=Tutorials.coverImageUrl,
                    RelatedService = new MaintenanceServiceDto
                    {
                        Id = Tutorials.RelatedService.Id,
                        ServiceName = Tutorials.RelatedService.ServiceName,
                        Description = Tutorials.RelatedService.Description,
                        Price = Tutorials.RelatedService.Price,
                        ImageUrl = Tutorials.RelatedService.ImageUrl
                    }

                }));
                    
                    
                    
                   
            }
            catch (Exception ex)
            {
                return new List<TutorialsDto>();

            }
        }

        public async Task<TutorialsDto> GetTutorialByIdAsync(int tutorialId)
        {
            try
            {
                var tutorial = await _context.Tutorials.Include(m => m.RelatedService).FirstOrDefaultAsync(t => t.Id == tutorialId);
                if(tutorial is null)
                {
                    return new TutorialsDto
                    {
                        Message = "Tutorial not found."
                    };
                }
                return new TutorialsDto
                {
                    Id = tutorial.Id,
                    Title = tutorial.Title,
                    Description = tutorial.Description,
                    coverImageUrl = tutorial.coverImageUrl,
                    PublishedDate = tutorial.PublishedDate,
                    RelatedService = new MaintenanceServiceDto
                    {
                        Id = tutorial.RelatedService.Id,
                        ServiceName = tutorial.RelatedService.ServiceName,
                        Description = tutorial.RelatedService.Description,
                        Price = tutorial.RelatedService.Price,
                        ImageUrl = tutorial.RelatedService.ImageUrl
                    },
                    Message = "Tutorial retrieved successfully."
                };

            }
            catch(Exception ex)
            {
                return new TutorialsDto
                {
                    Message = $"An error occurred while retrieving the tutorial: {ex.Message}"
                };
            }
        }

        public async Task<TutorialsDto> GetTutorialByNameAsync(string name)
        {
            try
            {
                var tutorial = await _context.Tutorials.Include(m => m.RelatedService).FirstOrDefaultAsync(t => t.Title.ToLower() == name.ToLower());
                if(tutorial is null)
                {
                    return new TutorialsDto
                    {
                        Message = "Tutorial not found."
                    };
                }
                return new TutorialsDto
                {
                    Id = tutorial.Id,
                    Title = tutorial.Title,
                    Description = tutorial.Description,
                    coverImageUrl = tutorial.coverImageUrl,
                    PublishedDate = tutorial.PublishedDate,
                    RelatedService = new MaintenanceServiceDto
                    {
                        Id = tutorial.RelatedService.Id,
                        ServiceName = tutorial.RelatedService.ServiceName,
                        Description = tutorial.RelatedService.Description,
                        Price = tutorial.RelatedService.Price,
                        ImageUrl = tutorial.RelatedService.ImageUrl
                    },
                    Message = "Tutorial retrieved successfully."
                };

            }
            catch(Exception ex)
            {
                return new TutorialsDto
                {
                    Message = $"An error occurred while retrieving the tutorial: {ex.Message}"
                };
            }
        }

        public async Task<TutorialsDto> UpdateTutorialAsync(int tutorialId, UpdateTutorialDto dto)
        {
            try
            {
                var exitingTutorial =  await _context.Tutorials.FirstOrDefaultAsync(t => t.Id == tutorialId);
                if (exitingTutorial is null)
                {
                    return new TutorialsDto
                    {
                        Message = "Tutorial not found."
                    };
                }
                var dtoProp=typeof(UpdateTutorialDto).GetProperties();
                var ModelProp=typeof(TutorialsModel).GetProperties();
                foreach(var prop in dtoProp)
                {
                    var value=dto.GetType().GetProperty(prop.Name)?.GetValue(dto,null);
                    if(value is not null)
                    {
                        ModelProp.FirstOrDefault(p=>p.Name==prop.Name)?.SetValue(exitingTutorial, value);
                    }
                }
               await _context.SaveChangesAsync();
                return new TutorialsDto
                {
                    Id = exitingTutorial.Id,
                    Title = exitingTutorial.Title,
                    Description = exitingTutorial.Description,
                    coverImageUrl = exitingTutorial.coverImageUrl,
                    PublishedDate = exitingTutorial.PublishedDate,
                    Message = "Tutorial updated successfully."
                };

            }
            catch(Exception ex)
            {
                return new TutorialsDto
                {
                    Message = $"An error occurred while updating the tutorial: {ex.Message}"
                };
            }
        }
    }
}
