namespace Infrastructure.Dtos;

public class ProjectDto
{
    public string ProjectId { get; set; } = string.Empty;
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Budget { get; set; }
}