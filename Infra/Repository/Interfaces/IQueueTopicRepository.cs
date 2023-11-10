using Models;

namespace Infra.Repository.Interfaces
{
    public interface IQueueTopicRepository : IARepository<QueueTopic>, IDisposable
    {
        void DeleteByTopics(IEnumerable<Topic> topics);
    }
}
