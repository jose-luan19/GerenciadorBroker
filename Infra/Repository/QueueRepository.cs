using Models;

namespace Infra.Repository
{
    public class QueueRepository : ARepository<Queues>
    {
        public QueueRepository(DbContextClass context) : base(context)
        {
        }
    }
}
