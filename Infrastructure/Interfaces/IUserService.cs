using Infrastructure.Dtos;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByEmailAsync(string email);
}