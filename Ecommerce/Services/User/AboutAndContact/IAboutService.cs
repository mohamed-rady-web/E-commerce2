using Ecommerce.Dtos.User.GetInTouch_About;

namespace Ecommerce.Services.User.AboutAndContact
{
    public interface IAboutService
    {
        public Task <AboutDto> GetAboutInfoAsync();
    }
}
