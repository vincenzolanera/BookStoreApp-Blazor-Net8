using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILogger<AuthController> _logger, IMapper _mapper, UserManager<ApiUser> _userManager, IConfiguration _config) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var resutl = await _userManager.CreateAsync(user, userDto.Password);

                if (!resutl.Succeeded)
                {
                    foreach (var error in resutl.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRoleAsync(user, "User");
                return Accepted();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Auth/{nameof(Register)} error";
                _logger.LogError(ex, errorMsg);
                return Problem(errorMsg);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
        {
            _logger.LogInformation($"Logint attempt for {userDto.Email}");
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);

                bool isPasswordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

                if (!isPasswordValid)
                {
                    return Unauthorized(userDto);
                }

                string token = await GenerateToken(user);
                
                var authResponse = new AuthResponse()
                {
                    Email = userDto.Email,
                    Token = token,
                    UserId = user.Id
                };

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Auth/{nameof(Login)} error";
                _logger.LogError(ex, errorMsg);
                return Problem(errorMsg);
            }
        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaims.UserId, user.Id),
            }
            .Union(roleClaims)
            .Union(userClaims);

            int tokenDurationMin = Convert.ToInt32(_config["JwtSettings:Duration"]);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(tokenDurationMin),
                signingCredentials: credentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
