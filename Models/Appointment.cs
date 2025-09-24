using System;

namespace HomecareAppointmentManagment.Models;

public class Appointment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public virtual Client Client { get; set; } = default!;
    public int HealthcareWorkerId { get; set; }
    public virtual HealthcareWorker HealthcareWorker { get; set; } = default!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Notes { get; set; } = string.Empty;
    public virtual List<AppointmentTask>? AppointmentTasks { get; set; }
}