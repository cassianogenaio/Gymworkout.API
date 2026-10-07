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

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

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

        modelBuilder.Entity<RefreshToken>(builder =>
        {
            builder.Property(token => token.TokenHash)
                .IsRequired()
                .HasMaxLength(64);

            builder.HasIndex(token => token.TokenHash)
                .IsUnique();

            builder.HasOne(token => token.User)
                .WithMany(user => user.RefreshTokens)
                .HasForeignKey(token => token.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}