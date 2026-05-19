namespace GymWorkout.API.DTOs.Workout;

public class WorkoutExerciseResponseDto
{
    public int Id { get; set; }

    public int WorkoutId { get; set; }

    public int ExerciseId { get; set; }

    public int Sets { get; set; }

    public int Reps { get; set; }

    public int RestTimeSeconds { get; set; }
}
