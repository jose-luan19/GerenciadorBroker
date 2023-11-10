using Models;

namespace Infra.Repository.Interfaces
{
    public interface IClientRepository : IDisposable, IARepository<Client>
    {
        Task<List<Client>> GetAllInclude();
    }
}
