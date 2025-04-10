using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, SignInManager<UserEntity> signInManager) : IUserService
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<UserDto?> RegisterUserAsync(UserDto userDto)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists");
            }

            var newUser = new UserEntity
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };

            var result = await _userRepository.CreateUserAsync(newUser, userDto.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }

            return MapToUserDto(newUser);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to register user: {ex.Message}", ex);
        }
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            return MapToUserDto(user);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get user by email: {ex.Message}", ex);
        }
    }

    public async Task<UserDto?> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            var isPasswordValid = await _userRepository.CheckPasswordAsync(user, password);
            if (!isPasswordValid) return null;

            await _signInManager.SignInAsync(user, isPersistent: false);

            return MapToUserDto(user);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to login user: {ex.Message}", ex);
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to logout user: {ex.Message}", ex);
        }
    }

    private static UserDto MapToUserDto(UserEntity user)
    {
        return new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
        };
    }
}