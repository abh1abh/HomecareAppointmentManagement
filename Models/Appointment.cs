using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Humanizer;

namespace HomecareAppointmentManagment.Models;

public class Appointment : IValidatableObject
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Client is required.")]
    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "Healthcare worker is required.")]
    public int HealthcareWorkerId { get; set; }

    public virtual HealthcareWorker HealthcareWorker { get; set; } = default!;

    [Display(Name = "Start time")]
    [DataType(DataType.DateTime)]
    [Required]
    public DateTime Start { get; set; }

    [Display(Name = "End time")]
    [DataType(DataType.DateTime)]
    [Required]
    public DateTime End { get; set; }

    [StringLength(1000, ErrorMessage = "Notes must be at most {1} characters.")]
    public string Notes { get; set; } = string.Empty;

    public int AvailableSlotId { get; set; } // Foreign key to AvailableSlot
    public virtual AvailableSlot AvailableSlot { get; set; } = default!;
    
    public virtual List<AppointmentTask>? AppointmentTasks { get; set; }

    // IValidatableObject implementation for cross validation
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (End <= Start)
        {
            yield return new ValidationResult(
                "End time must be after start time.",
                new[] { nameof(Start), nameof(End) });
        }

        if ((End - Start) != TimeSpan.FromHours(1))
        {
            yield return new ValidationResult(
                "Each appointment must be exactly 1 hour long.",
                new[] { nameof(Start), nameof(End) });
        }
    }
}
