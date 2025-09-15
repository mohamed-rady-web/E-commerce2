using Ecommerce.Dtos.User.TutorialsAndMaintenance;

namespace Ecommerce.Services.User.ServicesAndTutorials
{
    public interface ITutorialsService
    {
        public Task<List<TutorialsDto>> GetAllTutorialsAsync();
        public Task<TutorialsDto> GetTutorialByIdAsync(int tutorialId);
        public Task<TutorialsDto> CreateTutorialAsync(AddTutorialDto dto);
        public Task<TutorialsDto> UpdateTutorialAsync(int tutorialId, UpdateTutorialDto dto);
        public Task<TutorialsDto> DeleteTutorialAsync(int tutorialId);
        public Task<TutorialsDto> GetTutorialByNameAsync(string name);
    }
}
