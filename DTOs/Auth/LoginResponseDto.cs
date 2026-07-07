using System.ComponentModel.DataAnnotations;

namespace GymWorkout.API.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}