using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class AvailableSlotController : Controller
{
    private readonly IAvailableSlotRepository _repository;
    private readonly ILogger<AvailableSlotController> _logger; // Added

    public AvailableSlotController(IAvailableSlotRepository repository, ILogger<AvailableSlotController> logger) // Modified
    {
        _repository = repository;
        _logger = logger; // Added
    }

    public async Task<IActionResult> Index()
    {
        var slots = await _repository.GetAll();
        if (slots == null) // Added null check
        {
            _logger.LogError("[AvailableSlotController] available slot list not found while executing _repository.GetAll()");
            return NotFound("Available slot list not found");
        }
        return View(slots);
    }

    public async Task<IActionResult> Details(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null)
        {
            _logger.LogError("[AvailableSlotController] available slot not found while executing _repository.GetById() for AvailableSlotId {AvailableSlotId:0000}", id);
            return NotFound("Available slot not found");
        }
        return View(slot);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AvailableSlot slot)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Create(slot); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[AvailableSlotController] available slot creation failed {@slot}", slot);
        return View(slot);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null)
        {
            _logger.LogError("[AvailableSlotController] available slot not found when editing for AvailableSlotId {AvailableSlotId:0000}", id);
            return NotFound("Available slot not found");
        }
        return View(slot);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AvailableSlot slot)
    {
        if (id != slot.Id)
        {
            _logger.LogError("[AvailableSlotController] available slot ID mismatch during edit for AvailableSlotId {AvailableSlotId:0000}", id);
            return NotFound("Available slot ID mismatch");
        }
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Update(slot); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[AvailableSlotController] available slot update failed for AvailableSlotId {AvailableSlotId:0000}, {@slot}", id, slot);
        return View(slot);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var slot = await _repository.GetById(id);
        if (slot == null)
        {
            _logger.LogError("[AvailableSlotController] available slot not found when deleting for AvailableSlotId {AvailableSlotId:0000}", id);
            return NotFound("Available slot not found");
        }
        return View(slot);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _repository.Delete(id); // Modified
        if (!returnOk)
        {
            _logger.LogError("[AvailableSlotController] available slot deletion failed for AvailableSlotId {AvailableSlotId:0000}", id);
            return BadRequest("Available slot deletion failed");
        }
        return RedirectToAction(nameof(Index));
    }
}