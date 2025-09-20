using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.GetInTouch_About;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.AboutAndContact
{
    public class AboutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AboutService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

                return _mapper.Map<AboutDto>(aboutInfo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving about information: {ex.Message}");
            }
        }
    }
}
