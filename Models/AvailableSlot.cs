using System;

namespace HomecareAppointmentManagment.Models;

public class AvailableSlot
{
    public int Id { get; set; }
    public int HealthcarePersonnelId { get; set; }
    public virtual HealthcarePersonnel HealthcarePersonnel { get; set; } = default!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsBooked { get; set; }
    
}