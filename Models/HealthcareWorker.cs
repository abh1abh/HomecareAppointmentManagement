using System;

namespace HomecareAppointmentManagment.Models;

public class HealthcareWorker
{
    public int HealthcareWorkerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public virtual List<Appointment>? Appointments { get; set; }
    public virtual List<AvailableSlot>? AvailableSlots { get; set; }

}