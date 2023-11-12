using Models;

namespace Infra.Repository.Interfaces
{
    public interface IQueueRepository : IDisposable, IARepository<Queues>
    {
        Task<List<Queues>> GetAllInclude();
        Task<Queues> GetByIdIncludeClient(Guid id);
    }
}
