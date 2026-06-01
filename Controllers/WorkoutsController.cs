using System.Linq;
using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WorkoutResponseDto>>> Get()
    {
        var workoutDtos = (await _workoutService.GetWorkoutsAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(workoutDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutResponseDto>> GetById(int id)
    {
        var workout = await _workoutService.GetWorkoutByIdAsync(id);
        if (workout == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(workout));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkoutResponseDto>> Create(CreateWorkoutDto dto)
    {
        var workout = await _workoutService.CreateWorkoutAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = workout.Id }, ToResponseDto(workout));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutResponseDto>> Update(int id, UpdateWorkoutDto dto)
    {
        var updatedWorkout = await _workoutService.UpdateWorkoutAsync(id, dto);
        if (updatedWorkout == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updatedWorkout));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
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
