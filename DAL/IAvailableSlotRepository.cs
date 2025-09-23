using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagement.DAL;

public interface IAvailableSlotRepository
{
    Task<IEnumerable<AvailableSlot>> GetAll();
    Task<AvailableSlot?> GetById(int id);
    Task Create(AvailableSlot availableSlot);
    Task Update(AvailableSlot availableSlot);
    Task<bool> Delete(int id);
    Task<IEnumerable<AvailableSlot>> GetByHealthcarePersonnelId(int personnelId);
}