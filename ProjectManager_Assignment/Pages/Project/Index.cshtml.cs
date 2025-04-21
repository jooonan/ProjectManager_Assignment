using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectManager_Assignment.Models;

namespace ProjectManager_Assignment.Pages.Project;

[Authorize]
[ValidateAntiForgeryToken]
[RequireHttps]
public class IndexModel(IProjectService projectService, UserManager<UserEntity> userManager) : PageModel
{
    private readonly IProjectService _projectService = projectService;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public IEnumerable<ProjectDto> Projects { get; set; } = [];

    [BindProperty]
    public ProjectFormModel ProjectForm { get; set; } = new ProjectFormModel
    {
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(30)
    };

    public string CurrentTab { get; set; } = "all";
    public int AllCount { get; set; }
    public int StartedCount { get; set; }
    public int CompletedCount { get; set; }

    public async Task OnGetAsync(string tab = "all")
    {
        CurrentTab = tab;

        var allProjects = await _projectService.GetAllProjectsAsync();
        AllCount = allProjects.Count();

        var startedProjects = await _projectService.GetStartedProjectsAsync();
        StartedCount = startedProjects.Count();

        var completedProjects = await _projectService.GetCompletedProjectsAsync();
        CompletedCount = completedProjects.Count();

        switch (tab.ToLower())
        {
            case "started":
                Projects = startedProjects;
                break;
            case "completed":
                Projects = completedProjects;
                break;
            default:
                Projects = allProjects;
                break;
        }
    }

    public async Task<IActionResult> OnPostAddProjectAsync(bool isEdit = false, string projectId = "")
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var projectDto = new ProjectDto
            {
                ProjectId = isEdit ? projectId : string.Empty,
                ProjectName = ProjectForm.ProjectName,
                ClientName = ProjectForm.ClientName,
                Description = ProjectForm.Description ?? string.Empty,
                StartDate = ProjectForm.StartDate,
                EndDate = ProjectForm.EndDate,
                Budget = ProjectForm.Budget
            };

            if (isEdit)
            {
                await _projectService.UpdateProjectAsync(projectDto);
            }
            else
            {
                await _projectService.CreateProjectAsync(projectDto, user.Id);
            }

            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            await OnGetAsync();
            return Page();
        }
    }
    public async Task<IActionResult> OnGetProjectDetailsAsync(string projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
        {
            return NotFound();
        }

        return new JsonResult(project);
    }

    public async Task<IActionResult> OnPostDeleteAsync(string projectId)
    {
        await _projectService.DeleteProjectAsync(projectId);
        return RedirectToPage();
    }
}