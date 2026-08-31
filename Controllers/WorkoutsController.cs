using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.Workout;
using System.Security.Claims;

namespace GymWorkout.API.Controllers;

[Authorize]
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
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var workouts =  await _workoutService.GetWorkoutsAsync(userId);

        var workoutDtos = workouts
            .Select(ToResponseDto)
            .ToList();

        return Ok(workoutDtos);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<WorkoutResponseDto>>> GetAll()
    {
        var workoutDtos = (await _workoutService.GetAllWorkoutsAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(workoutDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutResponseDto>> GetById(int id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        
        var workout = await _workoutService.GetWorkoutByIdAsync(id);
        if (workout == null)
        {
            return NotFound();
        }

        if (!User.IsInRole("Admin") && workout.UserId != userId)
        {
            return Forbid();
        }

        return Ok(ToResponseDto(workout));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkoutResponseDto>> Create(CreateWorkoutDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        dto.UserId = userId;
        var workout = await _workoutService.CreateWorkoutAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = workout.Id }, ToResponseDto(workout));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutResponseDto>> Update(int id, UpdateWorkoutDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        dto.UserId = userId;
        var updatedWorkout = await _workoutService.UpdateWorkoutAsync(id, dto, userId);
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
                    ExerciseName = we.Exercise.Name,
                    Sets = we.Sets,
                    Reps = we.Reps,
                    RestTimeSeconds = we.RestTimeSeconds
                })
                .ToList()
        };
    }
}
