using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Infrastructure; // <-- add
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Authorization;           // <-- add
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

[Authorize(Roles = "HealthcareWorker,Admin")]
public class AvailableSlotController : Controller
{
    private readonly IAvailableSlotRepository _repository;
    private readonly ILogger<AvailableSlotController> _logger;

    public AvailableSlotController(IAvailableSlotRepository repository, ILogger<AvailableSlotController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsAdmin())
        {
            var all = await _repository.GetAll();
            return View(all);
        }

        var myId = User.TryGetHealthcareWorkerId();
        if (myId is null) return Forbid();

        var mine = await _repository.GetByWorkerId(myId.Value); // make sure this method returns a list
        return View(mine);
    }

    public async Task<IActionResult> Details(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null) return NotFound();

        if (!User.IsAdmin() && slot.HealthcareWorkerId != User.TryGetHealthcareWorkerId())
            return Forbid();

        return View(slot);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(AvailableSlot slot)
    {
        if (!ModelState.IsValid) return View(slot);

        if (!User.IsAdmin())
        {
            var myId = User.TryGetHealthcareWorkerId();
            if (myId is null) return Forbid();
            slot.HealthcareWorkerId = myId.Value; // force ownership
        }

        var ok = await _repository.Create(slot);
        if (!ok)
        {
            _logger.LogError("[AvailableSlotController] available slot creation failed {@slot}", slot);
            return View(slot);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null) return NotFound();

        if (!User.IsAdmin() && slot.HealthcareWorkerId != User.TryGetHealthcareWorkerId())
            return Forbid();

        return View(slot);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AvailableSlot slot)
    {
        if (id != slot.Id) return NotFound();

        var existing = await _repository.GetById(id);
        if (existing == null) return NotFound();

        if (!User.IsAdmin() && existing.HealthcareWorkerId != User.TryGetHealthcareWorkerId())
            return Forbid();

        // prevent switching ownership
        if (!User.IsAdmin()) slot.HealthcareWorkerId = existing.HealthcareWorkerId;

        if (!ModelState.IsValid) return View(slot);

        var ok = await _repository.Update(slot);
        if (!ok)
        {
            _logger.LogError("[AvailableSlotController] update failed {@slot}", slot);
            return View(slot);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null) return NotFound();

        if (!User.IsAdmin() && slot.HealthcareWorkerId != User.TryGetHealthcareWorkerId())
            return Forbid();

        return View(slot);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null) return NotFound();

        if (!User.IsAdmin() && slot.HealthcareWorkerId != User.TryGetHealthcareWorkerId())
            return Forbid();

        var ok = await _repository.Delete(id);
        if (!ok) return BadRequest("Available slot deletion failed");

        return RedirectToAction(nameof(Index));
    }
}
