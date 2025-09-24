

using HomecareAppointmentManagment.Models;

namespace HomecareAppointmentManagement.DAL;
public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAll();
    Task<Client?> GetClientById(int id);
    Task<bool> Create(Client client);
    Task<bool> Update(Client client);
    Task<bool> Delete(int id);
}