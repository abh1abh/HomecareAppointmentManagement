using HomecareAppointmentManagment.Models;


namespace HomecareAppointmentManagment.ViewModels;


public class AppointmentDetailsViewModel
{
    public AppointmentViewMode ViewMode { get; set; }
    public Appointment Appointment { get; set; } = new Appointment();

}
