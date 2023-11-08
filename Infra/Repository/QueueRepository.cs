using Infra.Repository.Interfaces;
using Models;

namespace Infra.Repository
{
    public class QueueRepository : ARepository<Queues>, IQueueRepository
    {
        public QueueRepository(DbContextClass context) : base(context)
        {
        }

        public bool ExistByName(string name)
        {
            return _dbSet.Any(x => x.Name == name);
        }
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
