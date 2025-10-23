using HomecareAppointmentManagement.Models;

namespace HomecareAppointmentManagement.DAL;
public interface IHealthcareWorkerRepository
{
    Task<IEnumerable<HealthcareWorker>?> GetAll();
    Task<HealthcareWorker?> GetById(int id);
    Task<bool> Create(HealthcareWorker healthcareWorker);
    Task<bool> Update(HealthcareWorker healthcareWorker);
    Task<bool> Delete(int id);
}