using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class HealthcareWorkerController : Controller
{
    private readonly IHealthcareWorkerRepository _repository;
    private readonly ILogger<HealthcareWorkerController> _logger; // Added

    public HealthcareWorkerController(IHealthcareWorkerRepository repository, ILogger<HealthcareWorkerController> logger) // Modified
    {
        _repository = repository;
        _logger = logger; // Added
    }

    public async Task<IActionResult> Table()
    {
        var healthcareWorkers = await _repository.GetAll();
        if (healthcareWorkers == null) // Added null check
        {
            _logger.LogError("[HealthcareWorkerController] healthcare workers list not found while executing _repository.GetAll()");
            return NotFound("Healthcare workers list not found");
        }
        return View(healthcareWorkers);
    }

    public async Task<IActionResult> Details(int id)
    {
        var worker = await _repository.GetById(id);
        if (worker == null)
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker not found while executing _repository.GetById() for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return NotFound("Healthcare worker not found");
        }
        return View(worker);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(HealthcareWorker worker)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Create(worker); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[HealthcareWorkerController] healthcare worker creation failed {@worker}", worker);
        return View(worker);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var worker = await _repository.GetById(id);
        if (worker == null)
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker not found when editing for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return NotFound("Healthcare worker not found");
        }
        return View(worker);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, HealthcareWorker worker)
    {
        if (id != worker.HealthcareWorkerId)
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker ID mismatch during edit for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return NotFound("Healthcare worker ID mismatch");
        }
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Update(worker); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[HealthcareWorkerController] healthcare worker update failed for HealthcareWorkerId {HealthcareWorkerId:0000}, {@worker}", id, worker);
        return View(worker);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var worker = await _repository.GetById(id);
        if (worker == null)
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker not found when deleting for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return NotFound("Healthcare worker not found");
        }
        return View(worker);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _repository.Delete(id); // Modified
        if (!returnOk)
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker deletion failed for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return BadRequest("Healthcare worker deletion failed");
        }
        return RedirectToAction(nameof(Index));
    }
}