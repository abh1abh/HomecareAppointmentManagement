using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class AppointmentTaskController : Controller
{
    private readonly IAppointmentTaskRepository _repository;
    private readonly ILogger<AppointmentTaskController> _logger; // Added

    public AppointmentTaskController(IAppointmentTaskRepository repository, ILogger<AppointmentTaskController> logger) // Modified
    {
        _repository = repository;
        _logger = logger; // Added
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _repository.GetAll();
        if (tasks == null) // Added null check
        {
            _logger.LogError("[AppointmentTaskController] appointment task list not found while executing _repository.GetAll()");
            return NotFound("Appointment task list not found");
        }
        return View(tasks);
    }

    public async Task<IActionResult> Details(int id)
    {
        var task = await _repository.GetById(id);
        if (task == null)
        {
            _logger.LogError("[AppointmentTaskController] appointment task not found while executing _repository.GetById() for AppointmentTaskId {AppointmentTaskId:0000}", id);
            return NotFound("Appointment task not found");
        }
        return View(task);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppointmentTask task)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Create(task); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[AppointmentTaskController] appointment task creation failed {@task}", task);
        return View(task);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _repository.GetById(id);
        if (task == null)
        {
            _logger.LogError("[AppointmentTaskController] appointment task not found when editing for AppointmentTaskId {AppointmentTaskId:0000}", id);
            return NotFound("Appointment task not found");
        }
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AppointmentTask task)
    {
        if (id != task.Id)
        {
            _logger.LogError("[AppointmentTaskController] appointment task ID mismatch during edit for AppointmentTaskId {AppointmentTaskId:0000}", id);
            return NotFound("Appointment task ID mismatch");
        }
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Update(task); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[AppointmentTaskController] appointment task update failed for AppointmentTaskId {AppointmentTaskId:0000}, {@task}", id, task);
        return View(task);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _repository.GetById(id);
        if (task == null)
        {
            _logger.LogError("[AppointmentTaskController] appointment task not found when deleting for AppointmentTaskId {AppointmentTaskId:0000}", id);
            return NotFound("Appointment task not found");
        }
        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _repository.Delete(id); // Modified
        if (!returnOk)
        {
            _logger.LogError("[AppointmentTaskController] appointment task deletion failed for AppointmentTaskId {AppointmentTaskId:0000}", id);
            return BadRequest("Appointment task deletion failed");
        }
        return RedirectToAction(nameof(Index));
    }
}