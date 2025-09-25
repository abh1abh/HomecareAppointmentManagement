using HomecareAppointmentManagment.Models;

public class AppointmentCreateViewModel
{
    public int? SelectedSlotId { get; set; }
    public IEnumerable<AvailableSlot> Slots { get; set; } = Enumerable.Empty<AvailableSlot>();
    public string? Notes { get; set; } // optional extra data
}
