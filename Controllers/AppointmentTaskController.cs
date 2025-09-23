using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

public class AppointmentTaskController : Controller
{
    private readonly IAppointmentTaskRepository _repository;

    public AppointmentTaskController(IAppointmentTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _repository.GetAll();
        return View(tasks);
    }

    public async Task<IActionResult> Details(int id)
    {
        var task = await _repository.GetById(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppointmentTask task)
    {
        await _repository.Create(task);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await _repository.GetById(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AppointmentTask task)
    {
        if (id != task.Id)
        {
            return NotFound();
        }
        await _repository.Update(task);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var task = await _repository.GetById(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}