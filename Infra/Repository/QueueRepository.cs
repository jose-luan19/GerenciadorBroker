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
           return _dbSet.Include(x => x.Client).OrderBy(x => x.CreateDate).ToList();
        }
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
