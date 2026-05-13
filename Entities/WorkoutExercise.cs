namespace GymWorkout.API.Entities;

public class WorkoutExercise
{
    public int Id { get; set; }

    // FK Workout
    public int WorkoutId { get; set; }

    public Workout Workout { get; set; } = null!;

    // FK Exercise
    public int ExerciseId { get; set; }

    public Exercise Exercise { get; set; } = null!;

    // Workout data
    public int Sets { get; set; }

    public int Reps { get; set; }

    public int RestTimeSeconds { get; set; }
}