using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class HealthcarePersonnelController : Controller
{
    private readonly IHealthcarePersonnelRepository _repository;

    public HealthcarePersonnelController(IHealthcarePersonnelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var personnel = await _repository.GetAll();
        return View(personnel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var person = await _repository.GetById(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(HealthcarePersonnel person)
    {
        await _repository.Create(person);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var person = await _repository.GetById(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, HealthcarePersonnel person)
    {
        if (id != person.HealthcarePersonnelId)
        {
            return NotFound();
        }
        await _repository.Update(person);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var person = await _repository.GetById(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}