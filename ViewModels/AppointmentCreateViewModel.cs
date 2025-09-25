using System.ComponentModel.DataAnnotations;
using HomecareAppointmentManagment.Models;


namespace HomecareAppointmentManagment.ViewModels;

public class AppointmentCreateViewModel
{
    public bool IsAdmin { get; set; } = false;

    [Required(ErrorMessage = "Please choose an available slot.")]
    public int? SelectedSlotId { get; set; }

    public IEnumerable<AvailableSlot> Slots { get; set; } = Enumerable.Empty<AvailableSlot>();

    [StringLength(1000, ErrorMessage = "Notes must be at most {1} characters.")]
    public string? Notes { get; set; }

    public IEnumerable<Client>? Clients { get; set; }

    // Only required for admins; enforce in controller before ModelState.IsValid
    public int? SelectedClientId { get; set; }

    public List<AppointmentTaskViewModel> AppointmentTasks { get; set; } =
        new() { new AppointmentTaskViewModel() };
}