using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HomecareAppointmentManagement.Models;

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

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string? Email { get; set; }

    public virtual List<Appointment>? Appointments { get; set; }
    public virtual List<AvailableSlot>? AvailableSlots { get; set; }

    public string? IdentityUserId { get; set; }
    public virtual IdentityUser? IdentityUser { get; set; }
}