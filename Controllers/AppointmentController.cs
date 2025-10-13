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
    private readonly IClientRepository _clientRepository;
    private readonly IAppointmentTaskRepository _appointmentTaskRepository;
    private readonly IChangeLogRepository _changeLogRepository;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController
    (
        IAppointmentRepository appointmentRepository,
        IAvailableSlotRepository availableSlotRepository,
        IClientRepository clientRepository,
        IAppointmentTaskRepository appointmentTaskRepository,
        IChangeLogRepository changeLogRepository,
        ILogger<AppointmentController> logger
    )
    {
        _appointmentRepository = appointmentRepository;
        _availableSlotRepository = availableSlotRepository;
        _clientRepository = clientRepository;
        _appointmentTaskRepository = appointmentTaskRepository;
        _changeLogRepository = changeLogRepository;
        _logger = logger;
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

    [Authorize(Roles = "Admin,Client")] // Restrict access to Admins and Clients, not HealthcareWorkers
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var slots = await _availableSlotRepository.GetAll(); // or a dedicated method like GetUnbookedFuture()

        var viewModel = new AppointmentCreateViewModel
        {
            IsAdmin = User.IsInRole("Admin"),
            Slots = (slots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start)
        };

        if (viewModel.IsAdmin)
        {
            viewModel.Clients = await _clientRepository.GetAll();
        }

        if (viewModel.AppointmentTasks == null || viewModel.AppointmentTasks.Count == 0)
        {
            viewModel.AppointmentTasks = new() { new AppointmentTaskViewModel() };
        }
        return View(viewModel);

    }

    [HttpPost]
    public async Task<IActionResult> Create(AppointmentCreateViewModel model)
    {
        async Task RehydrateAsync()
        {
            var allSlots = await _availableSlotRepository.GetAll();
            model.Slots = (allSlots ?? Enumerable.Empty<AvailableSlot>())
                .Where(s => !s.IsBooked && s.Start > DateTime.UtcNow)
                .OrderBy(s => s.Start)
                .ToList();

            model.IsAdmin = User.IsInRole("Admin");
            if (model.IsAdmin)
                model.Clients = (await _clientRepository.GetAll())?.ToList();

            if (model.AppointmentTasks == null || model.AppointmentTasks.Count == 0)
                model.AppointmentTasks = new() { new AppointmentTaskViewModel() };
        }

        // Flags first
        model.IsAdmin = User.IsInRole("Admin");

        // Normalize tasks once
        model.AppointmentTasks = (model.AppointmentTasks ?? new())
            .Where(t => !string.IsNullOrWhiteSpace(t.Description))
            .ToList();

        if (model.AppointmentTasks.Count == 0)
            ModelState.AddModelError("AppointmentTasks[0].Description", "Enter at least one task.");

        // Conditional admin requirement
        if (model.IsAdmin && model.SelectedClientId is null)
            ModelState.AddModelError(nameof(model.SelectedClientId),
                "Admin must specify a client to create an appointment for.");

        // Slot requirement (keep even if you have [Required] on VM for robustness)
        if (model.SelectedSlotId is null)
            ModelState.AddModelError(nameof(model.SelectedSlotId), "Please choose an available slot.");

        // One gate
        if (!ModelState.IsValid)
        {
            await RehydrateAsync();
            return View(model);
        }

        // From here on, assumptions hold: slot id and (if admin) client id exist
        var slot = await _availableSlotRepository.GetById(model.SelectedSlotId.Value);
        if (slot is null || slot.IsBooked || slot.Start <= DateTime.UtcNow)
        {
            ModelState.AddModelError(nameof(model.SelectedSlotId), "That slot is no longer available.");
            await RehydrateAsync();
            return View(model);
        }

        int? clientId = null;
        if (User.IsInRole("Client"))
        {
            clientId = User.TryGetClientId();
            if (clientId is null) return Forbid();
        }
        else if (model.IsAdmin)
        {
            // This should be non-null due to the earlier validation
            clientId = model.SelectedClientId;
        }

        var appointment = new Appointment
        {
            ClientId = clientId!.Value,
            HealthcareWorkerId = slot.HealthcareWorkerId,
            Start = slot.Start,
            End = slot.End,
            Notes = model.Notes ?? string.Empty
            // Set AvailableSlotId later, after booking the slot
        };


        // Book slot
        slot.IsBooked = true;
        var slotUpdated = await _availableSlotRepository.Update(slot);
        if (!slotUpdated)
        {
            _logger.LogError("[AppointmentController] failed to mark slot {SlotId} booked", slot.Id);
            ModelState.AddModelError("", "Could not book the selected slot. Please try another slot.");
            await RehydrateAsync();
            return View(model);
        }

        // Link appointment to slot
        appointment.AvailableSlotId = slot.Id;

        // Create appointment
        var created = await _appointmentRepository.Create(appointment);
        if (!created)
        {
            slot.IsBooked = false;
            await _availableSlotRepository.Update(slot);

            _logger.LogWarning("[AppointmentController] appointment creation failed {@appointment}", appointment);
            ModelState.AddModelError("", "Could not create appointment. Please try again.");
            await RehydrateAsync();
            return View(model);
        }

        // Create tasks
        foreach (var t in model.AppointmentTasks)
        {
            var ok = await _appointmentTaskRepository.Create(new AppointmentTask
            {
                AppointmentId = appointment.Id,
                Description = t.Description!,
                IsCompleted = false
            });

            if (!ok)
            {
                await _appointmentRepository.Delete(appointment.Id);
                slot.IsBooked = false;
                await _availableSlotRepository.Update(slot);

                ModelState.AddModelError(string.Empty, "Could not create tasks. Please try again.");
                await RehydrateAsync();
                return View(model);
            }
        }

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        if (appointment == null)
        {
            _logger.LogWarning("[AppointmentController] appointment not found when editing for AppointmentId {AppointmentId:0000}", id);
            return NotFound("Appointment not found");
        }

        var viewModel = new AppointmentEditViewModel
        {
            Id = appointment.Id,
            ClientName = appointment.Client?.Name ?? $"Client #{appointment.ClientId}",
            HealthcareWorkerName = appointment.HealthcareWorker?.Name ?? $"Worker #{appointment.HealthcareWorkerId}",
            Start = appointment.Start,
            End = appointment.End,
            Notes = appointment.Notes ?? string.Empty,
            AppointmentTasks = (appointment.AppointmentTasks ?? new List<AppointmentTask>())
                .Select(task => new AppointmentTaskEditItemViewModel
                {
                    Id = task.Id,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted
                })
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AppointmentEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("[AppointmentController] appointment update failed for AppointmentId {AppointmentId:0000}", model.Id);
            return View(model);
        }

        var existing = await _appointmentRepository.GetById(model.Id);
        if (existing == null)
        {
            _logger.LogError("[AppointmentController] appointment not found during edit for AppointmentId {AppointmentId:0000}", model.Id);
            return NotFound("Appointment not found");
        }

        var changes = new List<string>();

        if (!string.Equals(existing.Notes ?? string.Empty, model.Notes ?? string.Empty, StringComparison.Ordinal))
            changes.Add($"Notes: \"{existing.Notes}\" → \"{model.Notes}\"");

        existing.Notes = model.Notes ?? string.Empty;

        // App Task dict
        var existingById = existing.AppointmentTasks?.ToDictionary(t => t.Id, t => t);

        foreach (var viewModelTask in model.AppointmentTasks)
        {
            if (viewModelTask.Id.HasValue && existingById.TryGetValue(viewModelTask.Id.Value, out var t))
            {
                if (!string.Equals(t.Description, viewModelTask.Description, StringComparison.Ordinal) ||
                t.IsCompleted != viewModelTask.IsCompleted)
                {
                    changes.Add($"Task #{t.Id}: \"{t.Description}\"/{t.IsCompleted} → \"{viewModelTask.Description}\"/{viewModelTask.IsCompleted}");
                }
                t.Description = viewModelTask.Description;
                t.IsCompleted = viewModelTask.IsCompleted;
                await _appointmentTaskRepository.Update(t);
            }
            else
            {
                // New task added
                var newTask = new AppointmentTask
                {
                    AppointmentId = existing.Id,
                    Description = viewModelTask.Description,
                    IsCompleted = viewModelTask.IsCompleted
                };
                await _appointmentTaskRepository.Create(newTask);
                changes.Add($"Task + \"{viewModelTask.Description}\" (new)");
            }
        }

        // If nothing actually changed
        if (changes.Count == 0)
        {
            // Nothing to update/log; just go back
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        var updatedOk = await _appointmentRepository.Update(existing);
        if (!updatedOk)
        {
            _logger.LogWarning("[AppointmentController] appointment update failed for AppointmentId {AppointmentId:0000}", model.Id);
            return View(model);
        }

        var userId = User.TryGetUserId() ?? string.Empty; // Use "" or some sentinel if you cannot resolve a user id
        var description = string.Join("; ", changes);
        if (description.Length > 500) description = description[..500];


        var log = new ChangeLog
        {
            AppointmentId = model.Id,
            ChangeDate = DateTime.UtcNow,
            ChangedByUserId = userId,
            ChangeDescription = description
        };

        var logged = await _changeLogRepository.Create(log);
        if (!logged)
        {
            // Don't fail the whole edit even if change log doesn't happen
            _logger.LogWarning("[AppointmentController] failed to create change log for AppointmentId {AppointmentId:0000}. Description: {Description}", model.Id, description);
        }

        return RedirectToAction(nameof(Index));

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
        Appointment? appointment = await _appointmentRepository.GetById(id);
        if (appointment == null)
        {
            _logger.LogError("[AppointmentController] appointment not found when deleting for AppointmentId {AppointmentId:0000}", id);
            return NotFound("Appointment not found");
        }
        // Free up slot
        var slot = appointment.AvailableSlotId.HasValue
            ? await _availableSlotRepository.GetById(appointment.AvailableSlotId.Value)
            : null;
        
        if (slot != null)
        {
            slot.IsBooked = false;
            var slotOk = await _availableSlotRepository.Update(slot);
            if (!slotOk)
            {
                _logger.LogError("[AppointmentController] failed to free up slot for AppointmentId {AppointmentId:0000}", id);
            }
        }
        var userId = User.TryGetUserId() ?? string.Empty;

        bool logged = await _changeLogRepository.Create(new ChangeLog
        {
            AppointmentId = appointment.Id,
            ChangeDate = DateTime.UtcNow,
            ChangedByUserId = userId,
            ChangeDescription = "Appointment deleted."
        });

        if (!logged)
        {
            _logger.LogWarning("[AppointmentController] failed to create change log for deleted AppointmentId {AppointmentId:0000}", appointment.Id);
        }
        
        bool returnOk = await _appointmentRepository.Delete(appointment.Id); // Modified
        if (!returnOk)
        {
            _logger.LogError("[AppointmentController] appointment deletion failed for AppointmentId {AppointmentId:0000}", appointment.Id);
            return BadRequest("Appointment deletion failed");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ChangeLog(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        if (appointment == null) return NotFound("Appointment not found");

        // Authorization based on user  
        var isAdmin = User.IsInRole("Admin");
        var isClientOwner = User.IsInRole("Client") && User.TryGetClientId() == appointment.ClientId;
        var isWorkerOwner = User.IsInRole("HealthcareWorker") && User.TryGetHealthcareWorkerId() == appointment.HealthcareWorkerId;
        if (!(isAdmin || isClientOwner || isWorkerOwner)) return Forbid();

        var logs = await _changeLogRepository.GetByAppointmentId(id);

        ViewBag.AppointmentSummary =
        $"{appointment.Start:yyyy-MM-dd HH:mm} - {appointment.End:HH:mm}  Healthcare Worker: {appointment.HealthcareWorker?.Name ?? $"Worker #{appointment.HealthcareWorkerId}"}  Client: {appointment.Client?.Name ?? $"Worker #{appointment.ClientId}"} ";


        return View(logs?.OrderBy(l => l.ChangeDate));

    }
}


