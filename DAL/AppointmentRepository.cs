using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _db;

    public AppointmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Appointment>> GetAll()
    {
        return await _db.Appointments.ToListAsync();
    }

    public async Task<Appointment?> GetById(int id)
    {
        return await _db.Appointments.FindAsync(id);
    }

    public async Task Create(Appointment appointment)
    {
        await _db.Appointments.AddAsync(appointment);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Appointment appointment)
    {
        _db.Appointments.Update(appointment);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
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

    public async Task<IEnumerable<Appointment>> GetByClientId(int clientId)
    {
        return await _db.Appointments.Where(a => a.ClientId == clientId).ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByHealthcarePersonnelId(int personnelId)
    {
        return await _db.Appointments.Where(a => a.HealthcarePersonnelId == personnelId).ToListAsync();
    }
}