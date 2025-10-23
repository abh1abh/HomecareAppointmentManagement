using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class ChangeLogRepository : IChangeLogRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<ChangeLogRepository> _logger; 

    public ChangeLogRepository(AppDbContext db, ILogger<ChangeLogRepository> logger) 
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<ChangeLog>?> GetAll() 
    {
        try
        {
            return await _db.ChangeLogs.ToListAsync(); // Try to get all change logs
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<ChangeLog?> GetById(int id)
    {
        try
        {
            return await _db.ChangeLogs.FindAsync(id); // Try to find change log by ID
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log FindAsync(id) failed when GetById() for ChangeLogId {ChangeLogId:0000}, error messager: {e}", id, e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<bool> Create(ChangeLog changeLog) 
    {
        try
        {
            await _db.ChangeLogs.AddAsync(changeLog); // Add the change log
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<IEnumerable<ChangeLog>?> GetByAppointmentId(int appointmentId) 
    {
        try
        {
            return await _db.ChangeLogs.Where(c => c.AppointmentId == appointmentId).ToListAsync(); // Get change logs by appointment ID
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log Where(c => c.AppointmentId == appointmentId).ToListAsync() failed when GetByAppointmentId() for AppointmentId {AppointmentId:0000}, error messager: {e}", appointmentId, e.Message);
            return null; // Return null on failure
        }
    }
}