using HomecareAppointmentManagement.Models;

namespace HomecareAppointmentManagement.ViewModels;

    public class ClientViewModel
    {
        public IEnumerable<Client> Clients;
        public string? CurrentViewName;

        public ClientViewModel(IEnumerable<Client> clients, string? currentViewName)
        {
            Clients = clients;
            CurrentViewName = currentViewName;
        }
    }
