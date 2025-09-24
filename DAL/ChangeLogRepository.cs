using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class ChangeLogRepository : IChangeLogRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<ChangeLogRepository> _logger; // Added

    public ChangeLogRepository(AppDbContext db, ILogger<ChangeLogRepository> logger) // Modified
    {
        _db = db;
        _logger = logger; // Added
    }

    public async Task<IEnumerable<ChangeLog>?> GetAll() // Modified return type
    {
        try
        {
            return await _db.ChangeLogs.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null;
        }
    }

    public async Task<ChangeLog?> GetById(int id)
    {
        try
        {
            return await _db.ChangeLogs.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log FindAsync(id) failed when GetById() for ChangeLogId {ChangeLogId:0000}, error messager: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(ChangeLog changeLog) // Modified return type
    {
        try
        {
            await _db.ChangeLogs.AddAsync(changeLog);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false;
        }
    }

    public async Task<IEnumerable<ChangeLog>?> GetByAppointmentId(int appointmentId) // Modified return type
    {
        try
        {
            return await _db.ChangeLogs.Where(c => c.AppointmentId == appointmentId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ChangeLogRepository] change log Where(c => c.AppointmentId == appointmentId).ToListAsync() failed when GetByAppointmentId() for AppointmentId {AppointmentId:0000}, error messager: {e}", appointmentId, e.Message);
            return null;
        }
    }
}