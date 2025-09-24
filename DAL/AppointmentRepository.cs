using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<AppointmentRepository> _logger; // Added

    public AppointmentRepository(AppDbContext db, ILogger<AppointmentRepository> logger) // Modified
    {
        _db = db;
        _logger = logger; // Added
    }

    public async Task<IEnumerable<Appointment>?> GetAll() // Modified return type
    {
        try
        {
            return await _db.Appointments.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null;
        }
    }

    public async Task<Appointment?> GetById(int id)
    {
        try
        {
            return await _db.Appointments.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment FindAsync(id) failed when GetById() for AppointmentId {AppointmentId:0000}, error messager: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Appointment appointment) // Modified return type
    {
        try
        {
            await _db.Appointments.AddAsync(appointment);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Appointment appointment) // Modified return type
    {
        try
        {
            _db.Appointments.Update(appointment);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment Update() failed when Update() for AppointmentId {AppointmentId:0000}, error messager: {e}", appointment.Id, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.Appointments.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            _db.Appointments.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment Delete() failed when Delete() for AppointmentId {AppointmentId:0000}, error messager: {e}", id, e.Message);
            return false;
        }
    }

    public async Task<IEnumerable<Appointment>?> GetByClientId(int clientId) // Modified return type
    {
        try
        {
            return await _db.Appointments.Where(a => a.ClientId == clientId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment Where(a => a.ClientId == clientId).ToListAsync() failed when GetByClientId() for ClientId {ClientId:0000}, error messager: {e}", clientId, e.Message);
            return null;
        }
    }

    public async Task<IEnumerable<Appointment>?> GetByHealthcareWorkerId(int healthcareWorkerId) // Modified return type
    {
        try
        {
            return await _db.Appointments.Where(a => a.HealthcareWorkerId == healthcareWorkerId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[AppointmentRepository] appointment Where(a => a.HealthcareWorkerId == healthcareWorkerId).ToListAsync() failed when GetByHealthcareWorkerId() for WorkerId {WorkerId:0000}, error messager: {e}", healthcareWorkerId, e.Message);
            return null;
        }
    }
}