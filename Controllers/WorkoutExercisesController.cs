using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.WorkoutExercise;
using System.Security.Claims;

namespace GymWorkout.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WorkoutExercisesController : ControllerBase
{
    private readonly WorkoutExerciseService _workoutExerciseService;
    private readonly WorkoutService _workoutService;

    public WorkoutExercisesController(
        WorkoutExerciseService workoutExerciseService,
        WorkoutService workoutService)
    {
        _workoutExerciseService = workoutExerciseService;
        _workoutService = workoutService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WorkoutExerciseResponseDto>>> Get()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var workoutExercises = await _workoutExerciseService.GetWorkoutExercisesByUserAsync(userId);

        var dtos = workoutExercises
            .Select(ToResponseDto)
            .ToList();

        return Ok(dtos);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<WorkoutExerciseResponseDto>>> GetAll()
    {
        
        var workoutDtos = (await _workoutExerciseService.GetAllWorkoutsAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(workoutDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutExerciseResponseDto>> GetById(int id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var workoutExercise = await _workoutExerciseService.GetWorkoutExerciseByIdAsync(id);
        if (workoutExercise == null)
        {
            return NotFound();
        }

        var workout = await _workoutService.GetWorkoutByIdAsync(workoutExercise.WorkoutId);

        if (!User.IsInRole("Admin") && workout?.UserId != userId)
        {
            return Forbid();
        }
        
        return Ok(ToResponseDto(workoutExercise));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkoutExerciseResponseDto>> Create(CreateWorkoutExerciseDto dto)
    {
        var workoutExercise = await _workoutExerciseService.CreateWorkoutExerciseAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = workoutExercise.Id }, ToResponseDto(workoutExercise));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutExerciseResponseDto>> Update(int id, UpdateWorkoutExerciseDto dto)
    {
        var updated = await _workoutExerciseService.UpdateWorkoutExerciseAsync(id, dto);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updated));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
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
