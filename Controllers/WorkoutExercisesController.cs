using System.Linq;
using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WorkoutExerciseResponseDto>>> Get()
    {
        var dtos = (await _workoutExerciseService.GetWorkoutExercisesAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutExerciseResponseDto>> GetById(int id)
    {
        var workoutExercise = await _workoutExerciseService.GetWorkoutExerciseByIdAsync(id);
        if (workoutExercise == null)
        {
            return NotFound();
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
