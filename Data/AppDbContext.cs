using GymWorkout.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymWorkout.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Exercise> Exercises { get; set; } = null!;

    public DbSet<Workout> Workouts { get; set; } = null!;

    public DbSet<WorkoutExercise> WorkoutExercises { get; set; } = null!;
}