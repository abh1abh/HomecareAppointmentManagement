using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagment.Models;

public class Client
{
    public int ClientId { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name must be at most {1} characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string? Address { get; set; }

    [Required]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    public string? Phone { get; set; }

    public virtual List<Appointment>? Appointments { get; set; }
}

