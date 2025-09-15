using Ecommerce.Data;
using Ecommerce.Dtos.Auth;
using Ecommerce.Helpers;
using Ecommerce.Models.Cart;
using Ecommerce.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly ApplicationDbContext _context;


        public AuthService(UserManager<ApplicationUser> userManger, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, ApplicationDbContext context)
        {
            _userManger = userManger;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _context = context;
        }



        public async Task<AuthDto> RegisterAsync(RegisterDto dto)
        {
            try
            {
                if (await _userManger.FindByEmailAsync(dto.Email) is not null)
                {
                    return new AuthDto
                    {
                        Message = "Email already exists."
                    };
                }

                if (await _userManger.FindByNameAsync(dto.UserName) is not null)
                {
                    return new AuthDto
                    {
                        Message = "Username already exists."
                    };
                }

                var user = new ApplicationUser
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                };

                var result = await _userManger.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                {
                    return new AuthDto
                    {
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }
                await _userManger.AddToRoleAsync(user, "User");
                var cart = new CartModel
                {
                    UserId = user.Id,
                    CartItems = new List<CartItemModel>()
                };
                 _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                return new AuthDto
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    IsAuthenticated = true,
                    Roles = new List<string> { "User" },
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                return new AuthDto
                {
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }
        public async Task<AuthDto> LoginAsync(LoginDto dto)
        {
            var authModel = new AuthDto();
            var user = await _userManger.FindByEmailAsync(dto.Email);
            if (user is null || !await _userManger.CheckPasswordAsync(user, dto.Password))
            {
                authModel.Message = "Email or Password is incorrect.";
                return authModel;
            }
            var token = await GenerateJwtToken(user);
            var roles = await _userManger.GetRolesAsync(user);
            authModel.IsAuthenticated = true;
            authModel.Token = token;
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpireIn = DateTime.Now.AddDays(_jwt.DurationInDays);
            authModel.Roles = roles.ToList();
            authModel.Message = "Login successful.";
            return authModel;

        }
        public async Task<AuthDto> LogoutAsync(string userId)
        {
            var authDto = new AuthDto();

            try
            {
                var user = await _userManger.FindByIdAsync(userId);
                if (user == null)
                {
                    authDto.Message = "User not found.";
                    return authDto;
                }

                authDto.IsAuthenticated = false;
                authDto.Message = "User logged out successfully.";
                return authDto;
            }
            catch (Exception ex)
            {
                authDto.Message = $"An error occurred: {ex.Message}";
                return authDto;
            }
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {

            var userClaims = await _userManger.GetClaimsAsync(user);

            var roles = await _userManger.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));


            var claims = new List<Claim>
         {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            new Claim("FirstName", user.FirstName ?? ""),
            new Claim("LastName", user.LastName ?? ""),
            new Claim("UserId", user.Id),

         }
            .Union(userClaims)
            .Union(roleClaims);


            var symmetrickey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetrickey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}

