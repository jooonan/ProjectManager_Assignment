﻿using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UserRepository(UserManager<UserEntity> userManager)
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get user by email: {ex.Message}", ex);
        }
    }
}