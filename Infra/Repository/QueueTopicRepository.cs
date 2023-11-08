using Infra.Repository.Interfaces;
using Models;

namespace Infra.Repository
{
    public class QueueTopicRepository : ARepository<QueueTopic>, IQueueTopicRepository
    {
        public QueueTopicRepository(DbContextClass context) : base(context){}

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
