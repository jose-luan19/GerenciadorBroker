using Models;

namespace Infra.Repository.Interfaces
{
    public interface IClientTopicRepository : IDisposable, IARepository<ClientTopic>
    {
        Task<List<Client>> GetClientsByTopicId(Guid topicId);
    }
}
