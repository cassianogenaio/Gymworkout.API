using BCrypt.Net;
using GymWorkout.API.DTOs.User;
using GymWorkout.API.Entities;

namespace GymWorkout.API.Services;

public class UserService
{
    private readonly List<User> users = new();

    public List<User> GetUsers()
    {
        return users;
    }

    public User? GetUserById(int id)
    {
        return users.FirstOrDefault(u => u.Id == id);
    }

    public User? GetUserByEmail(string email)
    {
        return users.FirstOrDefault(u => u.Email == email);
    }

    public User CreateUser(CreateUserDto dto)
    {
        var user = new User
        {
            Id = users.Count + 1,
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        users.Add(user);

        return user;
}

    public User? UpdateUser(int id, UpdateUserDto dto)
    {
        var existingUser = users.FirstOrDefault(u => u.Id == id);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.Name = dto.Name;
        existingUser.Email = dto.Email;

        return existingUser;
    }

    public bool DeleteUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return false;
        }

        users.Remove(user);

        return true;
    }

}