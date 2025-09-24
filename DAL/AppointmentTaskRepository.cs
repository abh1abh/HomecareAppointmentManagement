using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AppointmentTaskRepository : IAppointmentTaskRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<AppointmentTaskRepository> _logger; // Added

    public AppointmentTaskRepository(AppDbContext db, ILogger<AppointmentTaskRepository> logger) // Modified
    {
        _db = db;
        _logger = logger; // Added
    }

    public async Task<IEnumerable<AppointmentTask>?> GetAll() // Modified return type
    {
        try
        {
            return await _db.AppointmentTasks.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null;
        }
    }

    public async Task<AppointmentTask?> GetById(int id)
    {
        try
        {
            return await _db.AppointmentTasks.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task FindAsync(id) failed when GetById() for AppointmentTaskId {AppointmentTaskId:0000}, error messager: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(AppointmentTask appointmentTask) // Modified return type
    {
        try
        {
            await _db.AppointmentTasks.AddAsync(appointmentTask);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false;
        }
    }

    public async Task<bool> Update(AppointmentTask appointmentTask) // Modified return type
    {
        try
        {
            _db.AppointmentTasks.Update(appointmentTask);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task Update() failed when Update() for AppointmentTaskId {AppointmentTaskId:0000}, error messager: {e}", appointmentTask.Id, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.AppointmentTasks.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            _db.AppointmentTasks.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task Delete() failed when Delete() for AppointmentTaskId {AppointmentTaskId:0000}, error messager: {e}", id, e.Message);
            return false;
        }
    }

    public async Task<IEnumerable<AppointmentTask>?> GetByAppointmentId(int appointmentId) // Modified return type
    {
        try
        {
            return await _db.AppointmentTasks.Where(a => a.AppointmentId == appointmentId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task Where(a => a.AppointmentId == appointmentId).ToListAsync() failed when GetByAppointmentId() for AppointmentId {AppointmentId:0000}, error messager: {e}", appointmentId, e.Message);
            return null;
        }
    }
}