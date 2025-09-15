using Ecommerce.Dtos.Auth;

namespace Ecommerce.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto model);
        Task<AuthDto> LoginAsync(LoginDto model);
        Task<AuthDto> LogoutAsync(string userId);

    }
}
