using HomecareAppointmentManagement.Models;

namespace HomecareAppointmentManagement.ViewModels;

public class AvailableSlotViewModel
{
    public bool IsAdmin { get; set; }
    public IEnumerable<AvailableSlot> AvailableSlots { get; set; } = Enumerable.Empty<AvailableSlot>();

}
