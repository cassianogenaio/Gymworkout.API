using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.Exercise;

namespace GymWorkout.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly ExerciseService _exerciseService;

    public ExercisesController(ExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ExerciseResponseDto>>> Get()
    {
        var exerciseDtos = (await _exerciseService.GetExercisesAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(exerciseDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseResponseDto>> GetById(int id)
    {
        var exercise = await _exerciseService.GetExerciseByIdAsync(id);
        if (exercise == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(exercise));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ExerciseResponseDto>> Create(CreateExerciseDto dto)
    {
        var exercise = await _exerciseService.CreateExerciseAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, ToResponseDto(exercise));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseResponseDto>> Update(int id, UpdateExerciseDto dto)
    {
        var updatedExercise = await _exerciseService.UpdateExerciseAsync(id, dto);

        if (updatedExercise == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updatedExercise));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _exerciseService.DeleteExerciseAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static ExerciseResponseDto ToResponseDto(Exercise exercise)
    {
        return new ExerciseResponseDto
        {
            Id = exercise.Id,
            Name = exercise.Name,
            MuscleGroup = exercise.MuscleGroup,
            Description = exercise.Description
        };
    }
}
