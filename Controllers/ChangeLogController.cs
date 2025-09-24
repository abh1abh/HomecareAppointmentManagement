using HomecareAppointmentManagement.DAL;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class ChangeLogController : Controller
{
    private readonly IChangeLogRepository _repository;
    private readonly ILogger<ChangeLogController> _logger; // Added

    public ChangeLogController(IChangeLogRepository repository, ILogger<ChangeLogController> logger) // Modified
    {
        _repository = repository;
        _logger = logger; // Added
    }

    public async Task<IActionResult> Index()
    {
        var logs = await _repository.GetAll();
        if (logs == null) // Added null check
        {
            _logger.LogError("[ChangeLogController] change log list not found while executing _repository.GetAll()");
            return NotFound("Change log list not found");
        }
        return View(logs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var log = await _repository.GetById(id);
        if (log == null)
        {
            _logger.LogError("[ChangeLogController] change log not found while executing _repository.GetById() for ChangeLogId {ChangeLogId:0000}", id);
            return NotFound("Change log not found");
        }
        return View(log);
    }
}