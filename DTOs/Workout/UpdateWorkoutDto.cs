namespace GymWorkout.API.DTOs.Workout;

public class UpdateWorkoutDto
{
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }
}
