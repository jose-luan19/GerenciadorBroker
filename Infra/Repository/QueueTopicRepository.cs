using Infra.Repository.Interfaces;
using Models;

namespace Infra.Repository
{
    public class QueueTopicRepository : ARepository<QueueTopic>, IQueueTopicRepository
    {
        public QueueTopicRepository(DbContextClass context) : base(context) { }

        public void DeleteByTopics(IEnumerable<Topic> topics)
        {
            foreach (var item in topics)
            {
                var queueTopicDelete = _dbSet.FirstOrDefault(x => x.TopicId == item.Id);
                Delete(queueTopicDelete);
            }
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
