using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectManager_Assignment.Models;

namespace ProjectManager_Assignment.Pages.Account;

[RequireHttps]
public class LoginModel(SignInManager<UserEntity> signInManager) : PageModel
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    [BindProperty]
    public LoginFormModel LoginForm { get; set; } = new();

    public string ReturnUrl { get; set; } = "/Projects/Index";

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        try
        {
            if (returnUrl != null)
                ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    LoginForm.Email,
                    LoginForm.Password,
                    LoginForm.RememberMe,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var user = await _signInManager.UserManager.FindByNameAsync(LoginForm.Email);
                    if (user != null)
                    {
                        HttpContext.Session.SetString("FullName", $"{user.FirstName} {user.LastName}");
                    }

                    return LocalRedirect(ReturnUrl);
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Account locked. Try again later.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Failed to process login: {ex.Message}");
            return Page();
        }
    }
}