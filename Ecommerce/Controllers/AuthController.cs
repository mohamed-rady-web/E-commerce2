using Ecommerce.Dtos.Auth;
using Ecommerce.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;
            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }
            [HttpPost("Register")]
            public async Task<IActionResult> Register([FromBody] RegisterDto dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    var result = await _authService.RegisterAsync(dto);
                    if (!result.IsAuthenticated)
                    {
                        return BadRequest(new { Message = result.Message });
                    }
                    return Ok(new
                    {
                        Message = result.Message,
                        UserName = result.UserName,
                        Email = result.Email,
                        Roles = result.Roles,
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
                }
            }
            [HttpPost("Login")]
            public async Task<IActionResult> Login([FromBody] LoginDto dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    var result = await _authService.LoginAsync(dto);
                    if (!result.IsAuthenticated)
                    {
                        return BadRequest(new { Message = result.Message });
                    }
                    return Ok(new
                    {
                        Message = result.Message,
                        UserName = result.UserName,
                        Email = result.Email,
                        Roles = result.Roles,
                        Token = result.Token,
                        Expiration = result.ExpireIn
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
                }
            }
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User not logged in." });

            var result = await _authService.LogoutAsync(userId);
            return Ok(result);
        }


    }
}




