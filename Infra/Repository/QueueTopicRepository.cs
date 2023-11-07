using Models;

namespace Infra.Repository
{
    public class QueueTopicRepository : ARepository<QueueTopic>
    {
        public QueueTopicRepository(DbContextClass context) : base(context)
        {
        }
    }
}
