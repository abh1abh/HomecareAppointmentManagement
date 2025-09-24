using HomecareAppointmentManagment.Models;


namespace HomecareAppointmentManagment.ViewModels;

public enum AppointmentIndexMode { Client, Worker, Admin }

public class AppointmentIndexViewModel
{
    public AppointmentIndexMode ViewMode { get; set; }
    public IEnumerable<Appointment> Appointments { get; set; } = Enumerable.Empty<Appointment>();

}
