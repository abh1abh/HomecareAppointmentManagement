using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AppointmentTaskRepository : IAppointmentTaskRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<AppointmentTaskRepository> _logger; 

    public AppointmentTaskRepository(AppDbContext db, ILogger<AppointmentTaskRepository> logger) 
    {
        _db = db;
        _logger = logger; 
    }

    public async Task<IEnumerable<AppointmentTask>?> GetAll() 
    {
        try
        {
            return await _db.AppointmentTasks.ToListAsync(); // Try to get all appointment tasks
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<AppointmentTask?> GetById(int id)
    {
        try
        {
            return await _db.AppointmentTasks.FindAsync(id); // Try to find appointment task by ID
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task FindAsync(id) failed when GetById() for AppointmentTaskId {AppointmentTaskId:0000}, error messager: {e}", id, e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<bool> Create(AppointmentTask appointmentTask) 
    {
        try
        {
            await _db.AppointmentTasks.AddAsync(appointmentTask); // Add the appointment task
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Update(AppointmentTask appointmentTask) 
    {
        try
        {
            _db.AppointmentTasks.Update(appointmentTask); // Update the appointment task
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task Update() failed when Update() for AppointmentTaskId {AppointmentTaskId:0000}, error messager: {e}", appointmentTask.Id, e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.AppointmentTasks.FindAsync(id); // Find the appointment task by ID
            if (item == null) // Check if the appointment task exists
            {
                return false; // Return false if not found
            }

            _db.AppointmentTasks.Remove(item);
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task Delete() failed when Delete() for AppointmentTaskId {AppointmentTaskId:0000}, error messager: {e}", id, e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<IEnumerable<AppointmentTask>?> GetByAppointmentId(int appointmentId) 
    {
        try
        {
            return await _db.AppointmentTasks.Where(a => a.AppointmentId == appointmentId).ToListAsync(); // Get appointment tasks by appointment ID
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentTaskRepository] appointment task Where(a => a.AppointmentId == appointmentId).ToListAsync() failed when GetByAppointmentId() for AppointmentId {AppointmentId:0000}, error messager: {e}", appointmentId, e.Message);
            return null; // Return null on failure
        }
    }
}