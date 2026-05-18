using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GymWorkout.API.Entities;
using GymWorkout.API.Services;
using GymWorkout.API.DTOs.User;

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

    // GET - listar usuários
    [HttpGet]
    public IActionResult Get()
    {
        var userDtos = _userService.GetUsers()
            .Select(ToResponseDto)
            .ToList();

        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(ToResponseDto(user));
    }

    // POST - criar usuário
    [HttpPost]
    public IActionResult Create(CreateUserDto dto)
    {
        var user = _userService.CreateUser(dto);
        return Ok(ToResponseDto(user));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateUserDto dto)
    {
        var updatedUser = _userService.UpdateUser(id, dto);

        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(ToResponseDto(updatedUser));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _userService.DeleteUser(id);

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