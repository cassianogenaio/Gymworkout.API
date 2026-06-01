using System.ComponentModel.DataAnnotations;

namespace GymWorkout.API.DTOs.Workout;

public class CreateWorkoutDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
}
