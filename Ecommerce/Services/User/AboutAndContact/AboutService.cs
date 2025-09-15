using Ecommerce.Data;
using Ecommerce.Dtos.User.GetInTouch_About;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.AboutAndContact
{
    public class AboutService
    {   private readonly ApplicationDbContext _context;
        public AboutService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<AboutDto> GetAboutInfoAsync()
        {
            try
            {
                var aboutInfo = await _context.Abouts.FirstOrDefaultAsync();
                if (aboutInfo == null)
                {
                    throw new Exception("About information not found.");
                }
                return new AboutDto
                {
                    Id = aboutInfo.Id,
                    CompanyName = aboutInfo.CompanyName,
                    Mission = aboutInfo.Mission,
                    Vision = aboutInfo.Vision,
                    History = aboutInfo.History,
                    Team = aboutInfo.Team,
                    ContactInfo = aboutInfo.ContactInfo
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving about information: {ex.Message}");
            }
        }
    }
}
