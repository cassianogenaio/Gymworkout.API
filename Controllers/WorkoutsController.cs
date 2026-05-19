using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.Workout;

namespace GymWorkout.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly WorkoutService _workoutService;

    public WorkoutsController(WorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var workoutDtos = (await _workoutService.GetWorkoutsAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(workoutDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var workout = await _workoutService.GetWorkoutByIdAsync(id);
        if (workout == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(workout));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkoutDto dto)
    {
        var workout = await _workoutService.CreateWorkoutAsync(dto);
        return Ok(ToResponseDto(workout));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateWorkoutDto dto)
    {
        var updatedWorkout = await _workoutService.UpdateWorkoutAsync(id, dto);
        if (updatedWorkout == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updatedWorkout));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _workoutService.DeleteWorkoutAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static WorkoutResponseDto ToResponseDto(Workout workout)
    {
        return new WorkoutResponseDto
        {
            Id = workout.Id,
            Name = workout.Name,
            UserId = workout.UserId,
            WorkoutExercises = workout.WorkoutExercises
                .Select(we => new WorkoutExerciseResponseDto
                {
                    Id = we.Id,
                    WorkoutId = we.WorkoutId,
                    ExerciseId = we.ExerciseId,
                    Sets = we.Sets,
                    Reps = we.Reps,
                    RestTimeSeconds = we.RestTimeSeconds
                })
                .ToList()
        };
    }
}
