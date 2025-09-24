using System;
using System.ComponentModel.DataAnnotations;

namespace HomecareAppointmentManagment.Models;

public class ChangeLog
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Appointment is required.")]
    public int AppointmentId { get; set; }

    public virtual Appointment Appointment { get; set; } = default!;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime ChangeDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ChangedByUserId is required.")]
    public int ChangedByUserId { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Change description must be at most {1} characters.")]
    public string ChangeDescription { get; set; } = string.Empty;
}