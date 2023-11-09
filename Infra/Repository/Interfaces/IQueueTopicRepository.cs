using Models;

namespace Infra.Repository.Interfaces
{
    public interface IQueueTopicRepository : IARepository<QueueTopic>, IDisposable
    {
        void DeleteByTopicid(IEnumerable<Topic> topics);
    }
}
