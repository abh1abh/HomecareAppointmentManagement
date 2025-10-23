using HomecareAppointmentManagement.Models;


namespace HomecareAppointmentManagement.ViewModels;

public enum AppointmentViewMode { Client, Worker, Admin }

public class AppointmentViewModel
{
    public AppointmentViewMode ViewMode { get; set; }
    public IEnumerable<Appointment> Appointments { get; set; } = Enumerable.Empty<Appointment>();

}
