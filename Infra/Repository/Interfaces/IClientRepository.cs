using Models;

namespace Infra.Repository.Interfaces
{
    public interface IClientRepository : IDisposable, IARepository<Client>
    {
        Task<List<Client>> GetAllInclude();
        Task<Client> GetDetailsById(Guid id);
        Task<List<Client>> GetAllOnline();
        Task<List<Client>> GetPossiblesContacts(Guid id);

    }
}
