using GymWorkout.API.Data;
using GymWorkout.API.DTOs.Workout;
using GymWorkout.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymWorkout.API.Services;

public class WorkoutService
{
    private readonly AppDbContext _context;

    public WorkoutService(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Workout>> GetWorkoutsAsync(int userId)
    {
        return _context.Workouts
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }

    public Task<List<Workout>> GetAllWorkoutsAsync()
    {
        return _context.Workouts
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
            .ToListAsync();
    }

    public Task<Workout?> GetWorkoutByIdAsync(int id)
    {
        return _context.Workouts
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Workout> CreateWorkoutAsync(CreateWorkoutDto dto)
    {
        var workout = new Workout
        {
            Name = dto.Name,
            UserId = dto.UserId,
            WorkoutExercises = new List<WorkoutExercise>()
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        return workout;
    }

    public async Task<Workout?> UpdateWorkoutAsync(int id, UpdateWorkoutDto dto, int userId)
    {
        var existingWorkout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
        if (existingWorkout == null)
        {
            return null;
        }

        existingWorkout.Name = dto.Name;
        existingWorkout.UserId = dto.UserId;

        await _context.SaveChangesAsync();
        return existingWorkout;
    }

    public async Task<bool> DeleteWorkoutAsync(int id)
    {
        var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == id);
        if (workout == null)
        {
            return false;
        }

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();
        return true;
    }
}
