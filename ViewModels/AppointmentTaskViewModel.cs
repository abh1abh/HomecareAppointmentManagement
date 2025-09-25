using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagment.ViewModels;

public class AppointmentTaskViewModel
{
    [Required(ErrorMessage = "Enter at least one task.")]
    [StringLength(200, ErrorMessage = "Description must be at most {1} characters.")]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;
}
