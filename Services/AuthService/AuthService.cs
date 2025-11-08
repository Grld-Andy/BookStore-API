
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Services;

public class AuthService(BookStoreContext context, IConfiguration configuration) : IAuthService
{
    public async Task<string?> LoginAsync(UserDto userDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == userDto.Username.ToLower());

        if(user is null)
        {
            return null;
        }
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userDto.Password) == PasswordVerificationResult.Failed)
        {
            return null;
        }

        string token = CreateToken(user);
        return token;
    }

    public async Task<User?> RegisterAsync(UserDto userDto)
    {
        if (await context.Users.AnyAsync(u => u.Username.ToLower() == userDto.Username.ToLower()))
        {
            return null;
        }

        var user = new User();

        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, userDto.Password);

        user.Username = userDto.Username;
        user.PasswordHash = hashedPassword;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
    
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
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