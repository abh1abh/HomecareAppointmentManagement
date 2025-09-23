

using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagement.DAL;
public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAll();
    Task<Client?> GetClientById(int id);
    Task Create(Client client);
    Task Update(Client client);
    Task<bool> Delete(int id);
}