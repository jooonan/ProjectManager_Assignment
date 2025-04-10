using Infrastructure.Dtos;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<UserDto?> RegisterUserAsync(UserDto userDto);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto?> LoginAsync(string email, string password);
    Task LogoutAsync();
}