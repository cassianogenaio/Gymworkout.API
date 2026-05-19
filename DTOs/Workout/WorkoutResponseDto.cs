namespace GymWorkout.API.DTOs.Workout;

public class WorkoutResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }

    public List<WorkoutExerciseResponseDto> WorkoutExercises { get; set; } = new();
}
