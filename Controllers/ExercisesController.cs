using System.Linq;
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
    public async Task<IActionResult> Get()
    {
        var exerciseDtos = (await _exerciseService.GetExercisesAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(exerciseDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var exercise = await _exerciseService.GetExerciseByIdAsync(id);
        if (exercise == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(exercise));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExerciseDto dto)
    {
        var exercise = await _exerciseService.CreateExerciseAsync(dto);
        return Ok(ToResponseDto(exercise));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateExerciseDto dto)
    {
        var updatedExercise = await _exerciseService.UpdateExerciseAsync(id, dto);

        if (updatedExercise == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updatedExercise));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
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
