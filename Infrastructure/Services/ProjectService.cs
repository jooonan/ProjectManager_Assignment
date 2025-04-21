using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ProjectService(ProjectRepository projectRepository, IClientService clientService) : IProjectService
{
    private readonly ProjectRepository _projectRepository = projectRepository;
    private readonly IClientService _clientService = clientService;

    public async Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto, string userId)
    {
        try
        {
            var clientDto = await _clientService.GetClientByNameAsync(projectDto.ClientName);
            if (clientDto == null)
            {
                clientDto = await _clientService.AddClientAsync(projectDto.ClientName);
            }

            var project = new ProjectEntity
            {
                ProjectName = projectDto.ProjectName,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Budget = projectDto.Budget,
                ClientId = clientDto.Id,
                UserId = userId
            };

            await _projectRepository.AddProjectAsync(project);

            return MapToProjectDto(project);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create project: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        try
        {
            var projects = await _projectRepository.GetAllProjectsAsync();
            return projects.Select(MapToProjectDto).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get all projects: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<ProjectDto>> GetStartedProjectsAsync()
    {
        try
        {
            var allProjects = await _projectRepository.GetAllProjectsAsync();
            var today = DateTime.Now.Date;

            var startedProjects = allProjects
                .Where(p => p.StartDate <= today && p.EndDate >= today)
                .Select(MapToProjectDto)
                .ToList();

            return startedProjects;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get started projects: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<ProjectDto>> GetCompletedProjectsAsync()
    {
        try
        {
            var allProjects = await _projectRepository.GetAllProjectsAsync();
            var today = DateTime.Now.Date;

            var completedProjects = allProjects
                .Where(p => p.EndDate < today)
                .Select(MapToProjectDto)
                .ToList();

            return completedProjects;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get completed projects: {ex.Message}", ex);
        }
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(string projectId)
    {
        try
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null) return null;

            return MapToProjectDto(project);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get project by ID: {ex.Message}", ex);
        }
    }

    public async Task UpdateProjectAsync(ProjectDto projectDto)
    {
        try
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectDto.ProjectId) ?? throw new Exception("Project not found");
            var clientDto = await _clientService.GetClientByNameAsync(projectDto.ClientName);

            if (clientDto == null)
            {
                clientDto = await _clientService.AddClientAsync(projectDto.ClientName);
            }

            project.ProjectName = projectDto.ProjectName;
            project.Description = projectDto.Description;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;
            project.Budget = projectDto.Budget;
            project.ClientId = clientDto.Id;

            await _projectRepository.UpdateProjectAsync(project);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update project: {ex.Message}", ex);
        }
    }

    public async Task DeleteProjectAsync(string projectId)
    {
        try
        {
            await _projectRepository.DeleteProjectAsync(projectId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete project: {ex.Message}", ex);
        }
    }

    // ChatGPT helped with this helper method to map ProjectEntity to ProjectDto
    private ProjectDto MapToProjectDto(ProjectEntity project)
    {
        return new()
        {
            ProjectId = project.ProjectId,
            ProjectName = project.ProjectName,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            ClientName = project.Client.ClientName
        };
    }
}