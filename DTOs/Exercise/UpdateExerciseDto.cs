using System.ComponentModel.DataAnnotations;

namespace GymWorkout.API.DTOs.Exercise;

public class UpdateExerciseDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string MuscleGroup { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
