using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static List<User> _users = new(); // In-memory storage for demo
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_users.Any(u => u.Email == user.Email))
                return BadRequest("User already exists");

            _users.Add(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var user = _users.FirstOrDefault(u => 
                u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            return Ok(new { message = $"Hello {email}, you are authenticated!" });
        }
    }
}
