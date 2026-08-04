using Microsoft.AspNetCore.Mvc;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace GymWorkout.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponseDto>>> Get()
    {
        var userDtos = (await _userService.GetUsersAsync())
            .Select(ToResponseDto)
            .ToList();

        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDto>> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(user));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> Create(CreateUserDto dto)
    {
        var user = await _userService.CreateUserAsync(dto);
        if (user == null)
        {
            return BadRequest("Email já existe. Use outro endereço ou faça login.");
        }

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, ToResponseDto(user));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDto>> Update(int id, UpdateUserDto dto)
    {
        var updatedUser = await _userService.UpdateUserAsync(id, dto);

        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updatedUser));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteUserAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static UserResponseDto ToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}