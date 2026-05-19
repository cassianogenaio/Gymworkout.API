using System.Linq;
using GymWorkout.API.Data;
using GymWorkout.API.DTOs.Exercise;
using GymWorkout.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymWorkout.API.Services;

public class ExerciseService
{
    private readonly AppDbContext _context;

    public ExerciseService(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Exercise>> GetExercisesAsync()
    {
        return _context.Exercises.ToListAsync();
    }

    public Task<Exercise?> GetExerciseByIdAsync(int id)
    {
        return _context.Exercises.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Exercise> CreateExerciseAsync(CreateExerciseDto dto)
    {
        var exercise = new Exercise
        {
            Name = dto.Name,
            MuscleGroup = dto.MuscleGroup,
            Description = dto.Description
        };

        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();

        return exercise;
    }

    public async Task<Exercise?> UpdateExerciseAsync(int id, UpdateExerciseDto dto)
    {
        var existingExercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        if (existingExercise == null)
        {
            return null;
        }

        existingExercise.Name = dto.Name;
        existingExercise.MuscleGroup = dto.MuscleGroup;
        existingExercise.Description = dto.Description;

        await _context.SaveChangesAsync();
        return existingExercise;
    }

    public async Task<bool> DeleteExerciseAsync(int id)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null)
        {
            return false;
        }

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
        return true;
    }
}
