using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.User;
using GymWorkout.API.DTOs.Auth;

namespace GymWorkout.API.Controllers;

[ApiController]
[Route("Auth")]
public class AuthController : ControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public AuthController(
        AuthService authService,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        _authService = authService;
        _configuration = configuration;
        _environment = environment;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register(CreateUserDto dto)
    {
        try
        {
            var user = await _authService.RegisterAsync(dto);
            if (user == null)
            {
                return BadRequest(new { erro = "Falha ao registrar usuário." });
            }

            var session = await _authService.CreateSessionAsync(user);
            SetRefreshTokenCookie(session.RefreshToken);
            var role = _authService.IsAdmin(user) ? "Admin" : "User";

            return Ok(new
            {
                Token = session.AccessToken,
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = role
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var user = await _authService.LoginAsync(dto.Email, dto.Password);
        if (user == null)
        {
            return Unauthorized(new { erro = "Email ou senha inválidos." });
        }

        var session = await _authService.CreateSessionAsync(user);
        SetRefreshTokenCookie(session.RefreshToken);
        var role = _authService.IsAdmin(user) ? "Admin" : "User";

        return Ok(new
        {
            Token = session.AccessToken,
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = role
        });
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
        {
            return Unauthorized(new { erro = "Sessão expirada. Faça login novamente." });
        }

        var session = await _authService.RefreshSessionAsync(refreshToken);
        if (session == null)
        {
            ClearRefreshTokenCookie();
            return Unauthorized(new { erro = "Sessão expirada. Faça login novamente." });
        }

        SetRefreshTokenCookie(session.RefreshToken);
        return Ok(new { Token = session.AccessToken });
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
        {
            await _authService.RevokeRefreshTokenAsync(refreshToken);
        }

        ClearRefreshTokenCookie();
        return NoContent();
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps || !_environment.IsDevelopment(),
            SameSite = SameSiteMode.Lax,
            Path = "/Auth",
            Expires = DateTimeOffset.UtcNow.AddDays(
                _configuration.GetValue<int?>("Jwt:RefreshTokenExpirationDays") ?? 7)
        });
    }

    private void ClearRefreshTokenCookie()
    {
        Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps || !_environment.IsDevelopment(),
            SameSite = SameSiteMode.Lax,
            Path = "/Auth"
        });
    }
}