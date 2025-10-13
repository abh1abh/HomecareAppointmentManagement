using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Infrastructure; // <-- add
using HomecareAppointmentManagment.Models;
using HomecareAppointmentManagment.ViewModels;
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
            return View(new AvailableSlotViewModel
            {
                IsAdmin = true,
                AvailableSlots = all ?? Enumerable.Empty<AvailableSlot>()
            });
        }

        var workerId = User.TryGetHealthcareWorkerId();
        if (workerId is null) return Forbid();

        var workerslots = await _repository.GetByWorkerId(workerId.Value);
        if (workerslots == null) {
            _logger.LogError("[AvailableSlotController] available slot list not found while executing _repository.GetByWorkerId() for HealthcareWorkerId {HealthcareWorkerId:0000}", workerId.Value);
            return NotFound();
        }
        return View(new AvailableSlotViewModel
        {
            IsAdmin = false,
            AvailableSlots = workerslots
        });
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AvailableSlot slot)
    {
        slot.IsBooked = false;

        if (!User.IsAdmin())
        {
            var myId = User.TryGetHealthcareWorkerId();
            if (myId is null) return Forbid();
            slot.HealthcareWorkerId = myId.Value; // Force the worker to be the logged-in user
            ModelState.Remove(nameof(AvailableSlot.HealthcareWorkerId)); // Remove validation for HealthcareWorkerId field (need to review)
        }
        // Need to review ModelState after removing the field validation
        if (!ModelState.IsValid) return View(slot);

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

        if (User.IsAdmin())
        {
            existing.HealthcareWorkerId = slot.HealthcareWorkerId;
            existing.Start = slot.Start;
            existing.End = slot.End;
            existing.IsBooked = slot.IsBooked;
        }
        else
        {
            // Worker rules
            if (!existing.IsBooked)
            {
                existing.Start = slot.Start;
                existing.End = slot.End;
                existing.IsBooked = false;
            }
        }
        
        var ok = await _repository.Update(existing);
        if (!ok)
        {
            _logger.LogError("[AvailableSlotController] update failed {@slot}", existing);
            return View(slot);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var existing = await _repository.GetById(id);
        if (existing == null) return NotFound();

        if (!User.IsAdmin() && existing.HealthcareWorkerId != User.TryGetHealthcareWorkerId())
            return Forbid();

        if (!existing.IsBooked)
        {
            TempData["Message"] = "Slot is already not booked.";
            return RedirectToAction(nameof(Details), new { id });
        }

        existing.IsBooked = false;
        var ok = await _repository.Update(existing);
        if (!ok)
        {
            _logger.LogError("[AvailableSlotController] cancel failed for {id}", id);
            return RedirectToAction(nameof(Details), new { id });
        }

        return RedirectToAction(nameof(Details), new { id });
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
        if (!ok)
        {
            _logger.LogError("[AvailableSlotController] available slot deletion failed for {id}", id);
            return BadRequest("Available slot deletion failed");
        }
        
        return RedirectToAction(nameof(Index));
    }
}
