using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class HealthcareWorkerRepository : IHealthcareWorkerRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<HealthcareWorkerRepository> _logger; // Added

    public HealthcareWorkerRepository(AppDbContext db, ILogger<HealthcareWorkerRepository> logger) // Modified
    {
        _db = db;
        _logger = logger; // Added
    }

    public async Task<IEnumerable<HealthcareWorker>?> GetAll() // Modified return type
    {
        try
        {
            return await _db.HealthcareWorkers.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null;
        }
    }

    public async Task<HealthcareWorker?> GetById(int id)
    {
        try
        {
            return await _db.HealthcareWorkers.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker FindAsync(id) failed when GetById() for HealthcareWorkerId {HealthcareWorkerId:0000}, error messager: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(HealthcareWorker healthcareWorker) // Modified return type
    {
        try
        {
            await _db.HealthcareWorkers.AddAsync(healthcareWorker);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcarePersonnelRepository] healthcare personnel AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false;
        }
    }

    public async Task<bool> Update(HealthcareWorker healthcareWorker) // Modified return type
    {
        try
        {
            _db.HealthcareWorkers.Update(healthcareWorker);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker Update() failed when Update() for HealthcareWorkerId {HealthcareWorkerId:0000}, error messager: {e}", healthcareWorker.HealthcareWorkerId, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.HealthcareWorkers.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            _db.HealthcareWorkers.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker Delete() failed when Delete() for HealthcareWorkerId {HealthcareWorkerId:0000}, error messager: {e}", id, e.Message);
            return false;
        }
    }
}