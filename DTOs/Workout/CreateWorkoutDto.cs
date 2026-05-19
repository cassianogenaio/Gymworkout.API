namespace GymWorkout.API.DTOs.Workout;

public class CreateWorkoutDto
{
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }
}
