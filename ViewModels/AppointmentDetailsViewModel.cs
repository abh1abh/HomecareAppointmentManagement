using HomecareAppointmentManagement.Models;


namespace HomecareAppointmentManagement.ViewModels;


public class AppointmentDetailsViewModel
{
    public AppointmentViewMode ViewMode { get; set; }
    public Appointment Appointment { get; set; } = new Appointment();

}
