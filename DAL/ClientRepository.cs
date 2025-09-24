using System;
using System.Collections.Generic;
using System.Linq;
using HomecareAppointmentManagment.Controllers;
using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomecareAppointmentManagement.DAL;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _db;
    private readonly ILogger<ClientRepository> _logger;

    public ClientRepository(AppDbContext db, ILogger<ClientRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        try
        {
            return await _db.Clients.ToListAsync();

        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return null;
        }
    }

    public async Task<Client?> GetClientById(int id)
    {
        try
        {
            return await _db.Clients.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client FindAsync(id) failed when GetClientById() for ClientId {ClientId:0000}, error messager: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Client client)
    {
        try
        {
            await _db.Clients.AddAsync(client);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false;
        }
    }

    public async Task<bool> Update(Client client)
    {
        try
        {
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client Update() failed when Update() for ClientId {ClientId:0000}, error messager: {e}", client.ClientId, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null) return false;

            _db.Clients.Remove(client);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client Delete() failed when Delete() for ClientId {ClientId:0000}, error messager: {e}", id, e.Message);
            return false;
        }
    }

} 
