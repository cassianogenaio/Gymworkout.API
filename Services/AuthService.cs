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

    public bool IsAdmin(User user)
    {
        return IsAdmin(user.Email);
    }

    public bool IsAdmin(string email)
    {
        var adminEmail = _configuration["Jwt:AdminEmail"] ?? string.Empty;
        return !string.IsNullOrWhiteSpace(email)
            && !string.IsNullOrWhiteSpace(adminEmail)
            && string.Equals(email.Trim(), adminEmail.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        if (IsAdmin(user))
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            claims.Add(new Claim("is_admin", "true"));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.Role, "User"));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
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

