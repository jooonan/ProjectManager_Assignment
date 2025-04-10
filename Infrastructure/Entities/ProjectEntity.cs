using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class ProjectEntity
{
    [Key]
    public string ProjectId { get; set; } = null!;

    [Required]
    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public decimal Budget { get; set; }

    [Required]
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;  // Changed from int to string
    public UserEntity User { get; set; } = null!;
}