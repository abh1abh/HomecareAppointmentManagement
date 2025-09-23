using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagement.DAL;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAll();
    Task<Appointment?> GetById(int id);
    Task Create(Appointment appointment);
    Task Update(Appointment appointment);
    Task<bool> Delete(int id);
    Task<IEnumerable<Appointment>> GetByClientId(int clientId);
    Task<IEnumerable<Appointment>> GetByHealthcarePersonnelId(int personnelId);
}