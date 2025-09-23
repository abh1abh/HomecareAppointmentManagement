using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentRepository _repository;

    public AppointmentController(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var appointments = await _repository.GetAll();
        return View(appointments);
    }

    public async Task<IActionResult> Details(int id)
    {
        var appointment = await _repository.GetById(id);
        if (appointment == null)
        {
            return NotFound();
        }
        return View(appointment);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        await _repository.Create(appointment);
        return RedirectToAction(nameof(Index));
    }

    
    public async Task<IActionResult> Edit(int id)
    {
        var appointment = await _repository.GetById(id);
        if (appointment == null)
        {
            return NotFound();
        }
        return View(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Appointment appointment)
    {
        if (id != appointment.Id)
        {
            return NotFound();
        }
        await _repository.Update(appointment);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _repository.GetById(id);
        if (appointment == null)
        {
            return NotFound();
        }
        return View(appointment);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}