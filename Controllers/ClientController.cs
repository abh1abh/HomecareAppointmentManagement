using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class ClientController : Controller
{
    
    List<Client> clients = new List<Client>
    {
        new Client { ClientId = 1, Name = "John Doe", Address = "123 Main St", Phone = "555-1234" },
        new Client { ClientId = 2, Name = "Jane Smith", Address = "456 Oak Ave", Phone = "555-5678" },
        new Client { ClientId = 3, Name = "Bob Johnson", Address = "789 Pine Rd", Phone = "555-9012" }
    };

   public IActionResult Table()
    {
        return View(clients);
    }
}

