using HomecareAppointmentManagement.Models;

namespace HomecareAppointmentManagement.DAL;

public interface IAppointmentTaskRepository
{
    Task<IEnumerable<AppointmentTask>?> GetAll();
    Task<AppointmentTask?> GetById(int id);
    Task<bool> Create(AppointmentTask appointmentTask);
    Task<bool> Update(AppointmentTask appointmentTask);
    Task<bool> Delete(int id);
    Task<IEnumerable<AppointmentTask>?> GetByAppointmentId(int appointmentId);
}