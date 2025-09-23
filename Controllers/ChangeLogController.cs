using HomecareAppointmentManagement.DAL;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class ChangeLogController : Controller
{
    private readonly IChangeLogRepository _repository;

    public ChangeLogController(IChangeLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var logs = await _repository.GetAll();
        return View(logs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var log = await _repository.GetById(id);
        if (log == null)
        {
            return NotFound();
        }
        return View(log);
    }
}