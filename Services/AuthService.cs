using System.Text;
using GymWorkout.API.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using GymWorkout.API.DTOs.User;
using GymWorkout.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace GymWorkout.API.Services;

public class AuthService
{   
    private readonly UserService _userService;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(UserService userService, AppDbContext context, IConfiguration configuration)
    {
        _userService = userService;
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthSession> CreateSessionAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashRefreshToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(GetRefreshTokenExpirationDays())
        });
        await _context.SaveChangesAsync();

        return new AuthSession(user, GenerateToken(user), refreshToken);
    }

    public async Task<AuthSession?> RefreshSessionAsync(string refreshToken)
    {
        var tokenHash = HashRefreshToken(refreshToken);
        var existingToken = await _context.RefreshTokens
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.TokenHash == tokenHash);

        if (existingToken == null || existingToken.RevokedAt != null || existingToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        existingToken.RevokedAt = DateTime.UtcNow;
        var replacementToken = GenerateRefreshToken();
        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = existingToken.UserId,
            TokenHash = HashRefreshToken(replacementToken),
            ExpiresAt = DateTime.UtcNow.AddDays(GetRefreshTokenExpirationDays())
        });
        await _context.SaveChangesAsync();

        return new AuthSession(existingToken.User, GenerateToken(existingToken.User), replacementToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var tokenHash = HashRefreshToken(refreshToken);
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(candidate => candidate.TokenHash == tokenHash && candidate.RevokedAt == null);

        if (token == null)
        {
            return;
        }

        token.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
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

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private static string HashRefreshToken(string refreshToken)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
    }

    private int GetRefreshTokenExpirationDays()
    {
        return _configuration.GetValue<int?>("Jwt:RefreshTokenExpirationDays") ?? 7;
    }
}

public sealed record AuthSession(User User, string AccessToken, string RefreshToken);

