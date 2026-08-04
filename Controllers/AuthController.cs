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
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

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

            var token = _authService.GenerateToken(user);
            return Ok(new { Token = token });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var user = await _authService.LoginAsync(dto.Email, dto.Password);
        if (user == null)
        {
            return Unauthorized(new { erro = "Email ou senha inválidos." });
        }

        var token = _authService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}