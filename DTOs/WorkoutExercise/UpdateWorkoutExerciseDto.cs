using System.ComponentModel.DataAnnotations;

namespace GymWorkout.API.DTOs.WorkoutExercise;

public class UpdateWorkoutExerciseDto
{
    [Range(1, int.MaxValue)]
    public int WorkoutId { get; set; }

    [Range(1, int.MaxValue)]
    public int ExerciseId { get; set; }

    [Range(1, 1000)]
    public int Sets { get; set; }

    [Range(1, 1000)]
    public int Reps { get; set; }

    [Range(0, 3600)]
    public int RestTimeSeconds { get; set; }
}
