using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class ClientEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ClientName { get; set; } = null!;

    // Navigation property
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
}