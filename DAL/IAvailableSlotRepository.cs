using HomecareAppointmentManagement.Models;

namespace HomecareAppointmentManagement.DAL;

public interface IAvailableSlotRepository
{
    Task<IEnumerable<AvailableSlot>?> GetAll();
    Task<IEnumerable<AvailableSlot>> GetByWorkerId(int healthcareWorkerId);
    Task<AvailableSlot?> GetById(int id);
    Task<bool> Create(AvailableSlot availableSlot);
    Task<bool> Update(AvailableSlot availableSlot);
    Task<bool> Delete(int id);
    Task<IEnumerable<AvailableSlot>?> GetByHealthcarePersonnelId(int personnelId);
}