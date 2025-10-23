using System;
using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagement.Models;

public class ChangeLog
{
    public int Id { get; set; }

    public int? AppointmentId { get; set; }

    public virtual Appointment Appointment { get; set; } = default!;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime ChangeDate { get; set; }

    [Required(ErrorMessage = "ChangedByUserId is required.")]
    public string ChangedByUserId { get; set; } = string.Empty;

    [Required]
    [StringLength(500, ErrorMessage = "Change description must be at most {1} characters.")]
    public string ChangeDescription { get; set; } = string.Empty;
}