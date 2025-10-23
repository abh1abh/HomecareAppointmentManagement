using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagement.ViewModels;

public class AppointmentEditViewModel
{
    [Required]
    public int Id { get; set; }

    // Read-only display
    public string ClientName { get; set; } = string.Empty;
    public string HealthcareWorkerName { get; set; } = string.Empty;

    [Display(Name = "Start time")]
    public DateTime Start { get; set; }

    [Display(Name = "End time")]
    public DateTime End { get; set; }

    // Editable
    [StringLength(1000, ErrorMessage = "Notes must be at most {1} characters.")]
    public string Notes { get; set; } = string.Empty;
    
    public List<AppointmentTaskEditItemViewModel> AppointmentTasks { get; set; } = []; 

}
