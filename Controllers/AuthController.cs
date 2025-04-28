using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Model;
using WebApplication1.Services;
using WebApplication1.Utilities;

namespace WebApplication1.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController(UserService userService, GenerateTokenService generateTokenService) : ControllerBase
    {
        private readonly UserService _userService = userService;
        private readonly GenerateTokenService _generateTokenService = generateTokenService;
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized();
            }
            return Ok(new
            {
                message = "Login successful",
                token = _generateTokenService.GenerateToken(user),
                role = user.Role
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userService.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            await _userService.AddAsync(user);
            return Ok(new
            {
                message = "Register successful",
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                }
            });
        }
    }
}
