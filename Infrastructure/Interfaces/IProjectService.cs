using Infrastructure.Dtos;

namespace Infrastructure.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto, string userId);
        Task DeleteProjectAsync(string projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<IEnumerable<ProjectDto>> GetCompletedProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(string projectId);
        Task<IEnumerable<ProjectDto>> GetStartedProjectsAsync();
        Task UpdateProjectAsync(ProjectDto projectDto);
    }
}