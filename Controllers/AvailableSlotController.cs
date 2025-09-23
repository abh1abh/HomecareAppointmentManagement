using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class AvailableSlotController : Controller
{
    private readonly IAvailableSlotRepository _repository;

    public AvailableSlotController(IAvailableSlotRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var slots = await _repository.GetAll();
        return View(slots);
    }

    public async Task<IActionResult> Details(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null)
        {
            return NotFound();
        }
        return View(slot);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AvailableSlot slot)
    {
        await _repository.Create(slot);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null)
        {
            return NotFound();
        }
        return View(slot);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AvailableSlot slot)
    {
        if (id != slot.Id)
        {
            return NotFound();
        }
        await _repository.Update(slot);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null)
        {
            return NotFound();
        }
        return View(slot);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}