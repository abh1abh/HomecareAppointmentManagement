using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagment.ViewModels;

public class AppointmentTaskEditItemViewModel
{
    public int? Id { get; set; } // null for new tasks

    [Required]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
}
