using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<AppointmentRepository> _logger; 

    public AppointmentRepository(AppDbContext db, ILogger<AppointmentRepository> logger) 
    {
        _db = db;
        _logger = logger; 
    }

    public async Task<IEnumerable<Appointment>?> GetAll() 
    {
        try
        {
            return await _db.Appointments.ToListAsync(); // Try to get all appointments
        }
        catch (Exception e) // Catch any exceptions
        {
            _logger.LogError("[AppointmentRepository] appointment ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<Appointment?> GetById(int id)
    {
        try
        {
            return await _db.Appointments.FindAsync(id); // Try to find appointment by ID
        }
        catch (Exception e) // Catch any exceptions
        {
            _logger.LogError("[AppointmentRepository] appointment FindAsync(id) failed when GetById() for AppointmentId {AppointmentId:0000}, error messager: {e}", id, e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<bool> Create(Appointment appointment) 
    {
        try
        {
            await _db.Appointments.AddAsync(appointment); // Add the appointment
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e) // Catch any exceptions
        {
            _logger.LogError("[AppointmentRepository] appointment AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Update(Appointment appointment) 
    {
        try
        {
            _db.Appointments.Update(appointment); // Update the appointment
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e) // Catch any exceptions
        {
            _logger.LogError("[AppointmentRepository] appointment Update() failed when Update() for AppointmentId {AppointmentId:0000}, error messager: {e}", appointment.Id, e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.Appointments.FindAsync(id); // Find the appointment by ID
            if (item == null) // Check if the appointment exists
            {
                return false; // Return false if not found
            }

            _db.Appointments.Remove(item);
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e) // Catch any exceptions
        {
            _logger.LogError("[AppointmentRepository] appointment Delete() failed when Delete() for AppointmentId {AppointmentId:0000}, error messager: {e}", id, e.Message);
            return false; // Return false on failure    
        }
    }

    public async Task<IEnumerable<Appointment>?> GetByClientId(int clientId) 
    {
        try
        {
            return await _db.Appointments.Where(a => a.ClientId == clientId).ToListAsync(); // Get appointments by client ID
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment Where(a => a.ClientId == clientId).ToListAsync() failed when GetByClientId() for ClientId {ClientId:0000}, error messager: {e}", clientId, e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<IEnumerable<Appointment>?> GetByHealthcareWorkerId(int healthcareWorkerId)
    {
        try
        {
            return await _db.Appointments.Where(a => a.HealthcareWorkerId == healthcareWorkerId).ToListAsync(); // Get appointments by healthcare worker ID
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment Where(a => a.HealthcareWorkerId == healthcareWorkerId).ToListAsync() failed when GetByHealthcareWorkerId() for WorkerId {WorkerId:0000}, error messager: {e}", healthcareWorkerId, e.Message);
            return null; // Return null on failure
        }
    }
}