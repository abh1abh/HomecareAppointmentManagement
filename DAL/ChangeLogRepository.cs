using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class ChangeLogRepository : IChangeLogRepository
{
    private readonly AppDbContext _db;

    public ChangeLogRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ChangeLog>> GetAll()
    {
        return await _db.ChangeLogs.ToListAsync();
    }

    public async Task<ChangeLog?> GetById(int id)
    {
        return await _db.ChangeLogs.FindAsync(id);
    }

    public async Task Create(ChangeLog changeLog)
    {
        await _db.ChangeLogs.AddAsync(changeLog);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ChangeLog>> GetByAppointmentId(int appointmentId)
    {
        return await _db.ChangeLogs.Where(c => c.AppointmentId == appointmentId).ToListAsync();
    }
}