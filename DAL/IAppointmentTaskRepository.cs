using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagement.DAL;

public interface IAppointmentTaskRepository
{
    Task<IEnumerable<AppointmentTask>> GetAll();
    Task<AppointmentTask?> GetById(int id);
    Task Create(AppointmentTask appointmentTask);
    Task Update(AppointmentTask appointmentTask);
    Task<bool> Delete(int id);
    Task<IEnumerable<AppointmentTask>> GetByAppointmentId(int appointmentId);
}