
namespace HomecareAppointmentManagment.Models;

public class Client
{
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public virtual List<Appointment>? Appointments { get; set; }

}