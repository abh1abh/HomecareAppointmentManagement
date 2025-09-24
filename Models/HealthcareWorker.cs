using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagment.Models;

public class HealthcareWorker
{
    public int HealthcareWorkerId { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name must be at most {1} characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200, ErrorMessage = "Address must be at most {1} characters.")]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    public virtual List<Appointment>? Appointments { get; set; }
    public virtual List<AvailableSlot>? AvailableSlots { get; set; }
}