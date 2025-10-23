using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagement.Controllers;


[Authorize(Roles = "Admin")]
public class HealthcareWorkerController : Controller
{
    private readonly IHealthcareWorkerRepository _repository;
    private readonly ILogger<HealthcareWorkerController> _logger; 

    public HealthcareWorkerController(IHealthcareWorkerRepository repository, ILogger<HealthcareWorkerController> logger) 
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IActionResult> Table()
    {
        var healthcareWorkers = await _repository.GetAll(); // Get all healthcare workers
        if (healthcareWorkers == null) 
        {
            _logger.LogError("[HealthcareWorkerController] healthcare workers list not found while executing _repository.GetAll()");
            return NotFound("Healthcare workers list not found");
        }
        return View(healthcareWorkers);
    }

    public async Task<IActionResult> Details(int id) // Get details of a specific healthcare worker
    {
        var worker = await _repository.GetById(id); // Get worker by ID
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
        if (ModelState.IsValid) // Checks if the model is valid
        {
            bool returnOk = await _repository.Create(worker); // Create the healthcare worker
            if (returnOk)
                return RedirectToAction(nameof(Index)); // Redirect to Index on success
        }
        _logger.LogError("[HealthcareWorkerController] healthcare worker creation failed {@worker}", worker); // Log error
        return View(worker);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) // Get healthcare worker for editing
    {
        var worker = await _repository.GetById(id); // Get worker by ID
        if (worker == null) // Check if worker exists
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker not found when editing for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return NotFound("Healthcare worker not found");
        }
        return View(worker);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, HealthcareWorker worker)
    {
        if (id != worker.HealthcareWorkerId) // ID mismatch check
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker ID mismatch during edit for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return NotFound("Healthcare worker ID mismatch");
        }
        if (ModelState.IsValid) // Checks if the model is valid
        {
            bool returnOk = await _repository.Update(worker); // Update the healthcare worker
            if (returnOk)
                return RedirectToAction(nameof(Index)); // Redirect to Index on success
        }
        _logger.LogError("[HealthcareWorkerController] healthcare worker update failed for HealthcareWorkerId {HealthcareWorkerId:0000}, {@worker}", id, worker);
        return View(worker); // Return the view with the worker model
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var worker = await _repository.GetById(id); // Get worker by ID
        if (worker == null) // Check if worker exists
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker not found when deleting for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return NotFound("Healthcare worker not found");
        }
        return View(worker); // Return the view with the worker model
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _repository.Delete(id); // Delete the healthcare worker
        if (!returnOk) // Check if deletion was unsuccessful, log error, and return BadRequest
        {
            _logger.LogError("[HealthcareWorkerController] healthcare worker deletion failed for HealthcareWorkerId {HealthcareWorkerId:0000}", id);
            return BadRequest("Healthcare worker deletion failed");
        }
        return RedirectToAction(nameof(Index)); // Redirect to Index on success
    }
}