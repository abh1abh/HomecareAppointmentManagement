using HomecareAppointmentManagement.Models;

namespace HomecareAppointmentManagement.DAL;

public interface IChangeLogRepository
{
    Task<IEnumerable<ChangeLog>?> GetAll();
    Task<ChangeLog?> GetById(int id);
    Task<bool> Create(ChangeLog changeLog);
    Task<IEnumerable<ChangeLog>?> GetByAppointmentId(int appointmentId);
}