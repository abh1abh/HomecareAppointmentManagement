using HomecareAppointmentManagment.DAL;
using HomecareAppointmentManagment.Models;
using HomecareAppointmentManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class ClientController : Controller
{
    
    private readonly AppDbContext _context;
    // List<Client> clients = new List<Client>
    // {
    //     new Client { ClientId = 1, Name = "John Doe", Address = "123 Main St", Phone = "555-1234" },
    //     new Client { ClientId = 2, Name = "Jane Smith", Address = "456 Oak Ave", Phone = "555-5678" },
    //     new Client { ClientId = 3, Name = "Bob Johnson", Address = "789 Pine Rd", Phone = "555-9012" }
    // };

    public ClientController(AppDbContext context)
    {
        _context = context;
    }

   public IActionResult Table()
    {
        List<Client> clients = _context.Clients.ToList();
        var clientsViewModel = new ClientViewModel(clients, "Table");
        return View(clientsViewModel);
    }
}

