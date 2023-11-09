using Models.ViewModel;
using Models;

namespace Services.Interfaces
{
    public interface ITopicService
    {
        Task CreateTopic(CreateTopicViewModel topicViewModel);
        Task DeleteTopic(string topicName);
        Task<IEnumerable<Topic>> GetAllTopics();
        Task TopicBindQueues(List<CreateQueueViewModel> queues, Topic topic);
    }
}
