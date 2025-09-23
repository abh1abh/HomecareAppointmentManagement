using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagement.DAL;
public interface IHealthcarePersonnelRepository
{
    Task<IEnumerable<HealthcarePersonnel>> GetAll();
    Task<HealthcarePersonnel?> GetById(int id);
    Task Create(HealthcarePersonnel healthcarePersonnel);
    Task Update(HealthcarePersonnel healthcarePersonnel);
    Task<bool> Delete(int id);
}