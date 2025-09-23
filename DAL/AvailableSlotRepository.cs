using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AvailableSlotRepository : IAvailableSlotRepository
{
    private readonly AppDbContext _db;

    public AvailableSlotRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AvailableSlot>> GetAll()
    {
        return await _db.AvailableSlots.ToListAsync();
    }

    public async Task<AvailableSlot?> GetById(int id)
    {
        return await _db.AvailableSlots.FindAsync(id);
    }

    public async Task Create(AvailableSlot availableSlot)
    {
        await _db.AvailableSlots.AddAsync(availableSlot);
        await _db.SaveChangesAsync();
    }

    public async Task Update(AvailableSlot availableSlot)
    {
        _db.AvailableSlots.Update(availableSlot);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
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

    public async Task<IEnumerable<AvailableSlot>> GetByHealthcarePersonnelId(int personnelId)
    {
        return await _db.AvailableSlots.Where(a => a.HealthcarePersonnelId == personnelId).ToListAsync();
    }
}