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
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAvailableSlotRepository _availableSlotRepository; 
    private readonly ILogger<AppointmentController> _logger; // Added

    public AppointmentController(IAppointmentRepository appointmentRepository, IAvailableSlotRepository availableSlotRepository, ILogger<AppointmentController> logger) // Modified
    {
        _appointmentRepository = appointmentRepository;
        _availableSlotRepository = availableSlotRepository; // Modified
        _logger = logger; // Added
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin"))
        {
            var all = await _appointmentRepository.GetAll();
            if (all == null) // Added null check
            {
                _logger.LogError("[AppointmentController] appointment list not found while executing _appointmentRepository.GetAll()");
                return NotFound("Appointment list not found");
            }
            return View(new AppointmentViewModel
            {
                ViewMode = AppointmentViewMode.Admin,
                Appointments = all
            });
        }

        if (User.IsInRole("Client"))
        {
            var clientId = User.TryGetClientId();
            if (clientId is null) return Forbid();
            var clientAppointments = await _appointmentRepository.GetByClientId(clientId.Value);
            if (clientAppointments == null) // Added null check
            {
                _logger.LogError("[AppointmentController] appointment list not found while executing _appointmentRepository.GetByClientId() for ClientId {ClientId:0000}", clientId.Value);
                return NotFound("Appointment list not found");
            }
            return View(new AppointmentViewModel
            {
                ViewMode = AppointmentViewMode.Client,
                Appointments = clientAppointments
            });
        }

        if (User.IsInRole("HealthcareWorker"))
        {
            var workerId = User.TryGetHealthcareWorkerId();
            if (workerId is null) return Forbid();
            var workerAppointments = await _appointmentRepository.GetByHealthcareWorkerId(workerId.Value);
            if (workerAppointments == null) // Added null check
            {
                _logger.LogError("[AppointmentController] appointment list not found while executing _appointmentRepository.GetByHealthcareWorkerId() for HealthcareWorkerId {HealthcareWorkerId:0000}", workerId.Value);
                return NotFound("Appointment list not found");
            }
            return View(new AppointmentViewModel
            {
                ViewMode = AppointmentViewMode.Worker,
                Appointments = workerAppointments
            });
        }

        return Forbid();
        
    }

    public async Task<IActionResult> Table()
    {
        var appointments = await _appointmentRepository.GetAll();
        if (appointments == null) // Added null check
        {
            _logger.LogError("[AppointmentController] appointment list not found while executing _appointmentRepository.GetAll()");
            return NotFound("Appointment list not found");
        }
        return View(appointments);
    }

    public async Task<IActionResult> Details(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        if (appointment == null)
        {
            _logger.LogError("[AppointmentController] appointment not found while executing _appointmentRepository.GetById() for AppointmentId {AppointmentId:0000}", id);
            return NotFound("Appointment not found");
        }
        return View(new AppointmentDetailsViewModel
        {
            Appointment = appointment,
            ViewMode = User.IsInRole("Admin") ? AppointmentViewMode.Admin : (User.IsInRole("Client") ? AppointmentViewMode.Client : AppointmentViewMode.Worker)
        });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var slots = await _availableSlotRepository.GetAll(); // or a dedicated method like GetUnbookedFuture()

        return View(new AppointmentCreateViewModel
        {
            Slots = (slots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppointmentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var allSlots = await _availableSlotRepository.GetAll();
            model.Slots = (allSlots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start);
            return View(model);
        }

        if (model.SelectedSlotId is null)
        {
            ModelState.AddModelError(nameof(model.SelectedSlotId), "Please choose an available slot.");
            var allSlots = await _availableSlotRepository.GetAll();
            model.Slots = (allSlots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start);
            return View(model);
        }

        var slot = await _availableSlotRepository.GetById(model.SelectedSlotId.Value);

        if (slot is null || slot.IsBooked || slot.Start <= DateTime.UtcNow)
        {
            ModelState.AddModelError(nameof(model.SelectedSlotId), "That slot is no longer available.");
            var allSlots = await _availableSlotRepository.GetAll();
            model.Slots = (allSlots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start);
            return View(model);
        }

        int? clientId = null;
        if (User.IsInRole("Client"))
        {
            clientId = User.TryGetClientId();
            if (clientId is null) return Forbid();
        }
        else if (User.IsInRole("Admin"))
        {
            // If you want Admin to create for a specific client, extend the view model with ClientId
            // For now, forbid if not provided:
            clientId = User.TryGetClientId(); // fallback if admins are also clients; otherwise, require VM.ClientId
            if (clientId is null)
            {
                ModelState.AddModelError("", "Admin must specify a client to create an appointment for.");
                var allSlots = await _availableSlotRepository.GetAll();
                model.Slots = (allSlots ?? Enumerable.Empty<AvailableSlot>())
                    .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                    .OrderBy(s => s.Start);
                return View(model);
            }
        }

        var appt = new Appointment
        {
            ClientId = clientId!.Value,
            HealthcareWorkerId = slot.HealthcareWorkerId,
            Start = slot.Start,
            End = slot.End,
            Notes = model.Notes
        };

        slot.IsBooked = true;
        var slotUpdated = await _availableSlotRepository.Update(slot);
        if (!slotUpdated)
        {
            _logger.LogError("[AppointmentController] failed to mark slot {SlotId} booked", slot.Id);
            ModelState.AddModelError("", "Could not book the selected slot. Please try another slot.");
            var allSlots = await _availableSlotRepository.GetAll();
            model.Slots = (allSlots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start);
            return View(model);
        }

        var created = await _appointmentRepository.Create(appt);
        if (!created) // rollback slot if appointment creation fails
        {
            
            slot.IsBooked = false;
            await _availableSlotRepository.Update(slot);

            _logger.LogError("[AppointmentController] appointment creation failed {@appointment}", appt);
            ModelState.AddModelError("", "Could not create appointment. Please try again.");
            var allSlots = await _availableSlotRepository.GetAll();
            model.Slots = (allSlots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start);
            return View(model);
        }

        return RedirectToAction(nameof(Index));
        // _logger.LogError("[AppointmentController] appointment creation failed {@appointment}", appointment);
        // return View(appointment);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
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
            bool returnOk = await _appointmentRepository.Update(appointment); // Modified
            if (returnOk)
                return RedirectToAction(nameof(Index));
        }
        _logger.LogError("[AppointmentController] appointment update failed for AppointmentId {AppointmentId:0000}, {@appointment}", id, appointment);
        return View(appointment);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
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
        bool returnOk = await _appointmentRepository.Delete(id); // Modified
        if (!returnOk)
        {
            _logger.LogError("[AppointmentController] appointment deletion failed for AppointmentId {AppointmentId:0000}", id);
            return BadRequest("Appointment deletion failed");
        }
        return RedirectToAction(nameof(Index));
    }
}