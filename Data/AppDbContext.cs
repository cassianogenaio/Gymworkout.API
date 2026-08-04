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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(256);
        });
    }
}