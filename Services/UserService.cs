using BCrypt.Net;
using GymWorkout.API.Data;
using GymWorkout.API.DTOs.User;
using GymWorkout.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymWorkout.API.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<User>> GetUsersAsync()
    {
        return _context.Users.ToListAsync();
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        var normalized = email?.Trim().ToLowerInvariant() ?? string.Empty;
        return _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == normalized);
    }

    public async Task<User?> CreateUserAsync(CreateUserDto dto)
    {
        var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
        var emailExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email.ToLower() == normalizedEmail);

        if (emailExists)
        {
            return null;
        }

        var user = new User
        {
            Name = dto.Name,
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            return null;
        }

        existingUser.Name = dto.Name;
        existingUser.Email = dto.Email;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
