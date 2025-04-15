using System.ComponentModel.DataAnnotations;

namespace ProjectManager_Assignment.Models;

public class ProjectFormModel
{
    [Display(Name = "Project Name", Prompt = "Project Name")]
    [Required(ErrorMessage = "Project name is required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Client name is required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    public string? Description { get; set; }

    [Display(Name = "Start Date", Prompt = "Start Date")]
    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "End Date")]
    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "Budget")]
    [Required(ErrorMessage = "Budget is required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid budget format")]
    public decimal Budget { get; set; }
}

