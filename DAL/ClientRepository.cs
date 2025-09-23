using System;
using System.Collections.Generic;
using System.Linq;
using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _db;

    public ClientRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        return await _db.Clients.ToListAsync();
    }

    public async Task<Client?> GetClientById(int id)
    {
        return await _db.Clients.FindAsync(id);
    }

    public async Task Create(Client client)
    {
        await _db.Clients.AddAsync(client);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Client client)
    {
        _db.Clients.Update(client);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _db.Clients.FindAsync(id);
        if (item == null)
        {
            return false;
        }

        _db.Clients.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
} 
