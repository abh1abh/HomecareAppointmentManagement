using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class AppointmentTaskRepository : IAppointmentTaskRepository
{
    private readonly AppDbContext _db;

    public AppointmentTaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AppointmentTask>> GetAll()
    {
        return await _db.AppointmentTasks.ToListAsync();
    }

    public async Task<AppointmentTask?> GetById(int id)
    {
        return await _db.AppointmentTasks.FindAsync(id);
    }

    public async Task Create(AppointmentTask appointmentTask)
    {
        await _db.AppointmentTasks.AddAsync(appointmentTask);
        await _db.SaveChangesAsync();
    }

    public async Task Update(AppointmentTask appointmentTask)
    {
        _db.AppointmentTasks.Update(appointmentTask);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
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

    public async Task<IEnumerable<AppointmentTask>> GetByAppointmentId(int appointmentId)
    {
        return await _db.AppointmentTasks.Where(a => a.AppointmentId == appointmentId).ToListAsync();
    }
}