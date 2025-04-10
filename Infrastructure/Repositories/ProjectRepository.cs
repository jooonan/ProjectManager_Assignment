using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddProjectAsync(ProjectEntity project)
    {
        try
        {
            project.ProjectId = await GenerateProjectIdAsync();
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to add project: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        try
        {
            return await _context.Projects
                .Include(p => p.Client)
                .Include(p => p.User)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get all projects: {ex.Message}", ex);
        }
    }

    public async Task<ProjectEntity?> GetProjectByIdAsync(string projectId)
    {
        try
        {
            return await _context.Projects
                .Include(p => p.Client)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get project by ID: {ex.Message}", ex);
        }
    }

    public async Task UpdateProjectAsync(ProjectEntity project)
    {
        try
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
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
            var project = await _context.Projects.FindAsync(projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete project: {ex.Message}", ex);
        }
    }

    private async Task<string> GenerateProjectIdAsync()
    {
        try
        {
            int counter = 101;
            string newId;
            bool exists;

            do
            {
                newId = $"P-{counter}";
                exists = await _context.Projects.AnyAsync(p => p.ProjectId == newId);
                counter++;
            }
            while (exists);

            return newId;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to generate project ID: {ex.Message}", ex);
        }
    }
}