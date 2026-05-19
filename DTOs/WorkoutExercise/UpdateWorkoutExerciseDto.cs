namespace GymWorkout.API.DTOs.WorkoutExercise;

public class UpdateWorkoutExerciseDto
{
    public int WorkoutId { get; set; }

    public int ExerciseId { get; set; }

    public int Sets { get; set; }

    public int Reps { get; set; }

    public int RestTimeSeconds { get; set; }
}
