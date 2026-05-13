namespace GymWorkout.API.Entities;

public class Workout
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;

    public List<WorkoutExercise> WorkoutExercises { get; set; } = [];
}
