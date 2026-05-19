using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.WorkoutExercise;

namespace GymWorkout.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkoutExercisesController : ControllerBase
{
    private readonly WorkoutExerciseService _workoutExerciseService;

    public WorkoutExercisesController(WorkoutExerciseService workoutExerciseService)
    {
        _workoutExerciseService = workoutExerciseService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dtos = (await _workoutExerciseService.GetWorkoutExercisesAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var workoutExercise = await _workoutExerciseService.GetWorkoutExerciseByIdAsync(id);
        if (workoutExercise == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(workoutExercise));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkoutExerciseDto dto)
    {
        var workoutExercise = await _workoutExerciseService.CreateWorkoutExerciseAsync(dto);
        return Ok(ToResponseDto(workoutExercise));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateWorkoutExerciseDto dto)
    {
        var updated = await _workoutExerciseService.UpdateWorkoutExerciseAsync(id, dto);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _workoutExerciseService.DeleteWorkoutExerciseAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static WorkoutExerciseResponseDto ToResponseDto(WorkoutExercise workoutExercise)
    {
        return new WorkoutExerciseResponseDto
        {
            Id = workoutExercise.Id,
            WorkoutId = workoutExercise.WorkoutId,
            ExerciseId = workoutExercise.ExerciseId,
            Sets = workoutExercise.Sets,
            Reps = workoutExercise.Reps,
            RestTimeSeconds = workoutExercise.RestTimeSeconds
        };
    }
}
