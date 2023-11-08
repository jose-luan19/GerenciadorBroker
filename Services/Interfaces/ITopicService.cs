using Models.ViewModel;
using Models;

namespace Services.Interfaces
{
    public interface ITopicService
    {
        Task CreateTopic(CreateTopicViewModel topicViewModel);
        Task DeleteTopic(Guid idTopic);
        Task<IEnumerable<Topic>> GetAllQueues();
    }
}
