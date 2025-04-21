using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository) : IUserService
{
    private readonly UserRepository _userRepository = userRepository;

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get user by email: {ex.Message}", ex);
        }
    }
}