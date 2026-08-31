using GymWorkout.API.Data;
using GymWorkout.API.DTOs.WorkoutExercise;
using GymWorkout.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymWorkout.API.Services;

public class WorkoutExerciseService
{
    private readonly AppDbContext _context;

    public WorkoutExerciseService(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<WorkoutExercise>> GetWorkoutExercisesAsync()
    {
        return _context.WorkoutExercises
            .Include(we => we.Workout)
            .ToListAsync();
    }

    public Task<List<WorkoutExercise>> GetWorkoutExercisesByUserAsync(int userId)
    {
        return _context.WorkoutExercises
            .Include(we => we.Workout)
            .Where(we => we.Workout.UserId == userId)
            .ToListAsync();
    }

    public Task<List<WorkoutExercise>> GetAllWorkoutsAsync()
    {
        return _context.WorkoutExercises
            .Include(we => we.Workout)
            .ToListAsync();
    }

    public Task<WorkoutExercise?> GetWorkoutExerciseByIdAsync(int id)
    {
        return _context.WorkoutExercises
            .Include(we => we.Workout)
            .FirstOrDefaultAsync(we => we.Id == id);
    }

    public async Task<WorkoutExercise> CreateWorkoutExerciseAsync(CreateWorkoutExerciseDto dto)
    {
        var workoutExercise = new WorkoutExercise
        {
            WorkoutId = dto.WorkoutId,
            ExerciseId = dto.ExerciseId,
            Sets = dto.Sets,
            Reps = dto.Reps,
            RestTimeSeconds = dto.RestTimeSeconds
        };

        _context.WorkoutExercises.Add(workoutExercise);
        await _context.SaveChangesAsync();
        return workoutExercise;
    }

    public async Task<WorkoutExercise?> UpdateWorkoutExerciseAsync(int id, UpdateWorkoutExerciseDto dto)
    {
        var existing = await _context.WorkoutExercises.FirstOrDefaultAsync(we => we.Id == id);
        if (existing == null)
        {
            return null;
        }

        existing.WorkoutId = dto.WorkoutId;
        existing.ExerciseId = dto.ExerciseId;
        existing.Sets = dto.Sets;
        existing.Reps = dto.Reps;
        existing.RestTimeSeconds = dto.RestTimeSeconds;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteWorkoutExerciseAsync(int id)
    {
        var workoutExercise = await _context.WorkoutExercises.FirstOrDefaultAsync(we => we.Id == id);
        if (workoutExercise == null)
        {
            return false;
        }

        _context.WorkoutExercises.Remove(workoutExercise);
        await _context.SaveChangesAsync();
        return true;
    }
}
