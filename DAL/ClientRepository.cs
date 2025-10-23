using System;
using System.Collections.Generic;
using System.Linq;
using HomecareAppointmentManagement.Controllers;
using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagement.Models;
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
            return await _db.Clients.ToListAsync(); // Try to get all clients

        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client ToListAsync() failed when GetAll(), error messager: {e}", e.Message);
            return new List<Client>(); // Return empty list on failure
        }
    }

    public async Task<Client?> GetClientById(int id)
    {
        try
        {
            return await _db.Clients.FindAsync(id); // Try to find client by ID
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client FindAsync(id) failed when GetClientById() for ClientId {ClientId:0000}, error messager: {e}", id, e.Message);
            return null; // Return null on failure
        }
    }

    public async Task<bool> Create(Client client)
    {
        try
        {
            await _db.Clients.AddAsync(client); // Add the new client
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client AddAsync() failed when Create(), error messager: {e}", e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Update(Client client)
    {
        try
        {
            _db.Clients.Update(client); // Update the client
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client Update() failed when Update() for ClientId {ClientId:0000}, error messager: {e}", client.ClientId, e.Message);
            return false; // Return false on failure
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var client = await _db.Clients.FindAsync(id); // Find the client by ID
            if (client == null) return false;  // Return false if client not found

            _db.Clients.Remove(client); // Remove the client
            await _db.SaveChangesAsync(); // Save changes to the database
            return true; // Return true on success
        }
        catch (Exception e)
        {
            _logger.LogError("[ClientRepository] client Delete() failed when Delete() for ClientId {ClientId:0000}, error messager: {e}", id, e.Message);
            return false; // Return false on failure
        }
    }

} 
