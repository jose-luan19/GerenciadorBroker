using Infra.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Repository
{
    public class QueueRepository : ARepository<Queues>, IQueueRepository
    {
        public QueueRepository(DbContextClass context) : base(context)
        {
        }
        public async Task<List<Queues>> GetAllInclude()
        {
            return _dbSet
                 .Include(q => q.Client)
                 .ToList(); ;
        }

        public async Task<Queues> GetByIdIncludeClient(Guid id)
        {
            return _dbSet.Include(x => x.Client).First(x => x.Id == id);
        }
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
