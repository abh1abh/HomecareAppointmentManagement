
namespace HomecareAppointmentManagment.Models;

public class ChangeLog
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public virtual Appointment Appointment { get; set; } = default!;
    public DateTime ChangeDate { get; set; }
    public int ChangedByUserId { get; set; } 
    public string ChangeDescription { get; set; } = string.Empty;
}