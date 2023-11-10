using Models;

namespace Infra.Repository.Interfaces
{
    public interface IQueueRepository : IDisposable, IARepository<Queues>
    {
    }
}
