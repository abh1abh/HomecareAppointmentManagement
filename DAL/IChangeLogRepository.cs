using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagement.DAL;

public interface IChangeLogRepository
{
    Task<IEnumerable<ChangeLog>> GetAll();
    Task<ChangeLog?> GetById(int id);
    Task Create(ChangeLog changeLog);
    Task<IEnumerable<ChangeLog>> GetByAppointmentId(int appointmentId);
}