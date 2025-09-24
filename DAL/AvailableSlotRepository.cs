using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AvailableSlotRepository : IAvailableSlotRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<AvailableSlotRepository> _logger; // Added

    public AvailableSlotRepository(AppDbContext db, ILogger<AvailableSlotRepository> logger) // Modified
    {
        _db = db;
        _logger = logger; // Added
    }

    public async Task<IEnumerable<AvailableSlot>?> GetAll() // Modified return type
    {
        try
        {
            return await _db.AvailableSlots.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null;
        }
    }

    public async Task<AvailableSlot?> GetById(int id)
    {
        try
        {
            return await _db.AvailableSlots.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot FindAsync(id) failed when GetById() for AvailableSlotId {AvailableSlotId:0000}, error messager: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(AvailableSlot availableSlot) // Modified return type
    {
        try
        {
            await _db.AvailableSlots.AddAsync(availableSlot);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false;
        }
    }

    public async Task<bool> Update(AvailableSlot availableSlot) // Modified return type
    {
        try
        {
            _db.AvailableSlots.Update(availableSlot);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot Update() failed when Update() for AvailableSlotId {AvailableSlotId:0000}, error messager: {e}", availableSlot.Id, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.AvailableSlots.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            _db.AvailableSlots.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot Delete() failed when Delete() for AvailableSlotId {AvailableSlotId:0000}, error messager: {e}", id, e.Message);
            return false;
        }
    }

    public async Task<IEnumerable<AvailableSlot>?> GetByHealthcarePersonnelId(int personnelId) // Modified return type
    {
        try
        {
            return await _db.AvailableSlots.Where(a => a.HealthcareWorkerId == personnelId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot Where(a => a.HealthcareWorkerId == personnelId).ToListAsync() failed when GetByHealthcareWorkerId() for WorkerId {WorkerId:0000}, error messager: {e}", personnelId, e.Message);
            return null;
        }
    }
}