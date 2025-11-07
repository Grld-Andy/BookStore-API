using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Namespace;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    public static User user = new();

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto userDto)
    {
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, userDto.Password);

        user.Username = userDto.Username;
        user.PasswordHash = hashedPassword;

        return Ok(user);
    }

    [HttpPost("login")]
    public ActionResult<string> Login(UserDto userDto)
    {
        if (user.Username == userDto.Username)
        {
            return BadRequest("User not found");
        }

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userDto.Password) == PasswordVerificationResult.Failed)
        {
            return BadRequest("Wrong password");
        }

        string token = "success";
        return Ok(token);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username)
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
