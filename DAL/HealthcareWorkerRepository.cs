using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class HealthcareWorkerRepository : IHealthcareWorkerRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<HealthcareWorkerRepository> _logger; 

    public HealthcareWorkerRepository(AppDbContext db, ILogger<HealthcareWorkerRepository> logger) 
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<HealthcareWorker>?> GetAll() 
    {
        try
        {
            return await _db.HealthcareWorkers.ToListAsync(); // Try to get all healthcare workers
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<HealthcareWorker?> GetById(int id)
    {
        try
        {
            return await _db.HealthcareWorkers.FindAsync(id); // Try to find healthcare worker by ID
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker FindAsync(id) failed when GetById() for HealthcareWorkerId {HealthcareWorkerId:0000}, error messager: {e}", id, e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<bool> Create(HealthcareWorker healthcareWorker) 
    {
        try
        {
            await _db.HealthcareWorkers.AddAsync(healthcareWorker); // Add the new healthcare worker
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcarePersonnelRepository] healthcare personnel AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Update(HealthcareWorker healthcareWorker) 
    {
        try
        {
            _db.HealthcareWorkers.Update(healthcareWorker); // Update the healthcare worker
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker Update() failed when Update() for HealthcareWorkerId {HealthcareWorkerId:0000}, error messager: {e}", healthcareWorker.HealthcareWorkerId, e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.HealthcareWorkers.FindAsync(id); // Find the healthcare worker by ID
            if (item == null) // If not found
            {
                return false; // Return false
            }

            _db.HealthcareWorkers.Remove(item); // Remove the healthcare worker
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[HealthcareWorkerRepository] healthcare worker Delete() failed when Delete() for HealthcareWorkerId {HealthcareWorkerId:0000}, error messager: {e}", id, e.Message);
            return false; // Return false on failure
        }
    }
}