using System.Text;
using GymWorkout.API.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using GymWorkout.API.DTOs.User;

namespace GymWorkout.API.Services;

public class AuthService
{   
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public AuthService(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    public async Task<User?> RegisterAsync(CreateUserDto dto)
    {
        var email = dto.Email?.Trim().ToLowerInvariant() ?? string.Empty;
        var existing = await _userService.GetUserByEmailAsync(email);
        if (existing != null)
        {
            throw new InvalidOperationException("Email já existe. Use outro endereço ou faça login.");
        }

        return await _userService.CreateUserAsync(dto);
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return user;
        }

        return null;
    }

    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpirationMinutes"]!)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

