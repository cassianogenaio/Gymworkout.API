namespace GymWorkout.API.DTOs.Exercise;

public class CreateExerciseDto
{
    public string Name { get; set; } = string.Empty;

    public string MuscleGroup { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
