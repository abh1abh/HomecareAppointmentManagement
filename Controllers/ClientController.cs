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

    public async Task<IActionResult> Details(int id)
    {
        var client = await _clientRepository.GetClientById(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(HomecareAppointmentManagment.Models.Client client)
    {
        await _clientRepository.Create(client);
        return RedirectToAction(nameof(Table));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var client = await _clientRepository.GetClientById(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, HomecareAppointmentManagment.Models.Client client)
    {
        if (id != client.ClientId)
        {
            return NotFound();
        }
        await _clientRepository.Update(client);
        return RedirectToAction(nameof(Table));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = await _clientRepository.GetClientById(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _clientRepository.Delete(id);
        return RedirectToAction(nameof(Table));
    }
}

