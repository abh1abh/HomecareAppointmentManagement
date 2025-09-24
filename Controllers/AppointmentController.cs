using HomecareAppointmentManagement.DAL;
using HomecareAppointmentManagment.Infrastructure;
using HomecareAppointmentManagment.Models;
using HomecareAppointmentManagment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers;

[Authorize(Roles = "Client,Admin,HealthcareWorker")]
public class AppointmentController : Controller
{
    private readonly IAppointmentRepository _repository;
    private readonly ILogger<AppointmentController> _logger; // Added

    public AppointmentController(IAppointmentRepository repository, ILogger<AppointmentController> logger) // Modified
    {
        _repository = repository;
        _logger = logger; // Added
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin"))
        {
            var all = await _repository.GetAll();
            if (all == null) // Added null check
            {
                _logger.LogError("[AppointmentController] appointment list not found while executing _repository.GetAll()");
                return NotFound("Appointment list not found");
            }
            return View(new AppointmentIndexViewModel
            {
                ViewMode = AppointmentIndexMode.Admin,
                Appointments = all
            });
        }

        if (User.IsInRole("Client"))
        {
            var clientId = User.TryGetClientId();
            if (clientId is null) return Forbid();
            var clientAppointments = await _repository.GetByClientId(clientId.Value);
            if (clientAppointments == null) // Added null check
            {
                _logger.LogError("[AppointmentController] appointment list not found while executing _repository.GetByClientId() for ClientId {ClientId:0000}", clientId.Value);
                return NotFound("Appointment list not found");
            }
            return View(new AppointmentIndexViewModel
            {
                ViewMode = AppointmentIndexMode.Client,
                Appointments = clientAppointments
            });
        }

        if (User.IsInRole("HealthcareWorker"))
        {
            var workerId = User.TryGetHealthcareWorkerId();
            if (workerId is null) return Forbid();
            var workerAppointments = await _repository.GetByHealthcareWorkerId(workerId.Value);
            if (workerAppointments == null) // Added null check
            {
                _logger.LogError("[AppointmentController] appointment list not found while executing _repository.GetByHealthcareWorkerId() for HealthcareWorkerId {HealthcareWorkerId:0000}", workerId.Value);
                return NotFound("Appointment list not found");
            }
            return View(new AppointmentIndexViewModel
            {
                ViewMode = AppointmentIndexMode.Worker,
                Appointments = workerAppointments
            });
        }

        return Forbid();
        
    }

    public async Task<IActionResult> Table()
    {
        var appointments = await _repository.GetAll();
        if (appointments == null) // Added null check
        {
            _logger.LogError("[AppointmentController] appointment list not found while executing _repository.GetAll()");
            return NotFound("Appointment list not found");
        }
        return View(appointments);
    }

    public async Task<IActionResult> Details(int id)
    {
        var appointment = await _repository.GetById(id);
        if (appointment == null)
        {
            _logger.LogError("[AppointmentController] appointment not found while executing _repository.GetById() for AppointmentId {AppointmentId:0000}", id);
            return NotFound("Appointment not found");
        }
        return View(appointment);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Create(appointment); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[AppointmentController] appointment creation failed {@appointment}", appointment);
        return View(appointment);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var appointment = await _repository.GetById(id);
        if (appointment == null)
        {
            _logger.LogError("[AppointmentController] appointment not found when editing for AppointmentId {AppointmentId:0000}", id);
            return NotFound("Appointment not found");
        }
        return View(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Appointment appointment)
    {
        if (id != appointment.Id)
        {
            _logger.LogError("[AppointmentController] appointment ID mismatch during edit for AppointmentId {AppointmentId:0000}", id);
            return NotFound("Appointment ID mismatch");
        }
        if (ModelState.IsValid)
        {
            bool returnOk = await _repository.Update(appointment); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[AppointmentController] appointment update failed for AppointmentId {AppointmentId:0000}, {@appointment}", id, appointment);
        return View(appointment);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _repository.GetById(id);
        if (appointment == null)
        {
            _logger.LogError("[AppointmentController] appointment not found when deleting for AppointmentId {AppointmentId:0000}", id);
            return NotFound("Appointment not found");
        }
        return View(appointment);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool returnOk = await _repository.Delete(id); // Modified
        if (!returnOk)
        {
            _logger.LogError("[AppointmentController] appointment deletion failed for AppointmentId {AppointmentId:0000}", id);
            return BadRequest("Appointment deletion failed");
        }
        return RedirectToAction(nameof(Index));
    }
}