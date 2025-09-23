using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class ClientController : Controller
{
    
    private readonly IClientRepository _clientRepository;

    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

   public async Task<IActionResult> Table()
    {
        var clients = await _clientRepository.GetAll();
        var clientsViewModel = new ClientViewModel(clients, "Table");
        return View(clientsViewModel);
    }
}

