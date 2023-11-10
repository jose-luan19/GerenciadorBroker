using Models;
using Models.ViewModel;

namespace Services.Interfaces
{
    public interface ITopicService
    {
        Task CreateTopic(CreateTopicViewModel topicViewModel);
        Task DeleteTopic(string topicName);
        Task<List<ReadAllTopicsViewModel>> GetAllTopics();
        Task TopicBindQueues(List<CreateQueueViewModel> queues, Topic topic);
    }
}
