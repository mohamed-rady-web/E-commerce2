using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Models.User.ServicesAndTutorials;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.ServicesAndTutorials
{
    public class TutorialsService : ITutorialsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TutorialsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TutorialsDto> CreateTutorialAsync(AddTutorialDto dto)
        {
            try
            {
                var existingTutorial = await _context.Tutorials
                    .FirstOrDefaultAsync(t => t.Title.ToLower() == dto.Title.ToLower());

                if (existingTutorial != null)
                {
                    return new TutorialsDto { Message = "Tutorial with the same title already exists." };
                }

                var newTutorial = _mapper.Map<TutorialsModel>(dto);
                newTutorial.PublishedDate = DateTime.UtcNow;

                _context.Tutorials.Add(newTutorial);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<TutorialsDto>(newTutorial);
                result.Message = "Tutorial created successfully.";
                return result;
            }
            catch (Exception ex)
            {
                return new TutorialsDto
                {
                    Message = $"An error occurred while creating the tutorial: {ex.Message}"
                };
            }
        }

        public async Task<TutorialsDto> DeleteTutorialAsync(int tutorialId)
        {
            try
            {
                var existingTutorial = await _context.Tutorials.FirstOrDefaultAsync(t => t.Id == tutorialId);
                if (existingTutorial is null)
                {
                    return new TutorialsDto { Message = "Tutorial not found." };
                }

                _context.Tutorials.Remove(existingTutorial);
                await _context.SaveChangesAsync();

                return new TutorialsDto { Message = "Tutorial deleted successfully." };
            }
            catch (Exception ex)
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
                var tutorials = await _context.Tutorials
                    .Include(m => m.RelatedService)
                    .ToListAsync();

                return _mapper.Map<List<TutorialsDto>>(tutorials);
            }
            catch
            {
                return new List<TutorialsDto>();
            }
        }

        public async Task<TutorialsDto> GetTutorialByIdAsync(int tutorialId)
        {
            try
            {
                var tutorial = await _context.Tutorials
                    .Include(m => m.RelatedService)
                    .FirstOrDefaultAsync(t => t.Id == tutorialId);

                if (tutorial is null)
                {
                    return new TutorialsDto { Message = "Tutorial not found." };
                }

                var result = _mapper.Map<TutorialsDto>(tutorial);
                result.Message = "Tutorial retrieved successfully.";
                return result;
            }
            catch (Exception ex)
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
                var tutorial = await _context.Tutorials
                    .Include(m => m.RelatedService)
                    .FirstOrDefaultAsync(t => t.Title.ToLower() == name.ToLower());

                if (tutorial is null)
                {
                    return new TutorialsDto { Message = "Tutorial not found." };
                }

                var result = _mapper.Map<TutorialsDto>(tutorial);
                result.Message = "Tutorial retrieved successfully.";
                return result;
            }
            catch (Exception ex)
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
                var existingTutorial = await _context.Tutorials
                    .Include(t => t.RelatedService)
                    .FirstOrDefaultAsync(t => t.Id == tutorialId);

                if (existingTutorial is null)
                {
                    return new TutorialsDto { Message = "Tutorial not found." };
                }

                _mapper.Map(dto, existingTutorial);

                await _context.SaveChangesAsync();

                var result = _mapper.Map<TutorialsDto>(existingTutorial);
                result.Message = "Tutorial updated successfully.";
                return result;
            }
            catch (Exception ex)
            {
                return new TutorialsDto
                {
                    Message = $"An error occurred while updating the tutorial: {ex.Message}"
                };
            }
        }
    }
}
