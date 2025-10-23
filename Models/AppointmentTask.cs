using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagement.Models;

public class AppointmentTask
{
    public int Id { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Appointment is required.")]
    public int AppointmentId { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Description must be at most {1} characters.")]
    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public virtual Appointment Appointment { get; set; } = default!;
}

