using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AvailableSlotRepository : IAvailableSlotRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<AvailableSlotRepository> _logger; 

    public AvailableSlotRepository(AppDbContext db, ILogger<AvailableSlotRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<AvailableSlot>?> GetAll()   
    {
        try
        {
            return await _db.AvailableSlots.ToListAsync(); // Try to get all available slots
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null; // Return null on failure
        }
    }


    public async Task<IEnumerable<AvailableSlot>> GetByWorkerId(int healthcareWorkerId)
    {
        try
        {
            var q = _db.AvailableSlots.Where(s => s.HealthcareWorkerId == healthcareWorkerId); // Query slots for the specified healthcare worker
            return await q.OrderBy(s => s.Start).ToListAsync(); // Return ordered list of slots

        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot Where(healthcareWorkerId) failed when GetByWorkerId() for HealthcareWorkerId {HealthcareWorkerId:0000}, error messager: {e}",
                healthcareWorkerId, e.Message);
            return new List<AvailableSlot>();  // Return empty list on failure
        }
    }

    public async Task<AvailableSlot?> GetById(int id)
    {
        try
        {
            return await _db.AvailableSlots.FindAsync(id); // Try to find available slot by ID
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot FindAsync(id) failed when GetById() for AvailableSlotId {AvailableSlotId:0000}, error messager: {e}", id, e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<bool> Create(AvailableSlot availableSlot) 
    {
        try
        {
            await _db.AvailableSlots.AddAsync(availableSlot); // Add the available slot
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Update(AvailableSlot availableSlot) 
    {
        try
        {
            _db.AvailableSlots.Update(availableSlot); // Update the available slot
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot Update() failed when Update() for AvailableSlotId {AvailableSlotId:0000}, error messager: {e}", availableSlot.Id, e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.AvailableSlots.FindAsync(id); // Find the available slot by ID
            if (item == null) // Check if the available slot exists
            {
                return false; // Return false if not found
            }

            _db.AvailableSlots.Remove(item); // Remove the available slot
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot Delete() failed when Delete() for AvailableSlotId {AvailableSlotId:0000}, error messager: {e}", id, e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<IEnumerable<AvailableSlot>?> GetByHealthcarePersonnelId(int personnelId) 
    {
        try
        {
            return await _db.AvailableSlots.Where(a => a.HealthcareWorkerId == personnelId).ToListAsync(); // Get available slots by healthcare worker ID
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableSlotRepository] available slot Where(a => a.HealthcareWorkerId == personnelId).ToListAsync() failed when GetByHealthcareWorkerId() for WorkerId {WorkerId:0000}, error messager: {e}", personnelId, e.Message);
            return null; // Return null on failure
        }
    }
}