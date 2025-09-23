using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class HealthcarePersonnelRepository : IHealthcarePersonnelRepository
{
    private readonly AppDbContext _db;

    public HealthcarePersonnelRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<HealthcarePersonnel>> GetAll()
    {
        return await _db.HealthcarePersonnel.ToListAsync();
    }

    public async Task<HealthcarePersonnel?> GetById(int id)
    {
        return await _db.HealthcarePersonnel.FindAsync(id);
    }

    public async Task Create(HealthcarePersonnel healthcarePersonnel)
    {
        await _db.HealthcarePersonnel.AddAsync(healthcarePersonnel);
        await _db.SaveChangesAsync();
    }

    public async Task Update(HealthcarePersonnel healthcarePersonnel)
    {
        _db.HealthcarePersonnel.Update(healthcarePersonnel);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _db.HealthcarePersonnel.FindAsync(id);
        if (item == null)
        {
            return false;
        }

        _db.HealthcarePersonnel.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}