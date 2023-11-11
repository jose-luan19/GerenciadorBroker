using Infra.Repository.Interfaces;
using Models;

namespace Infra.Repository
{
    public class ClientTopicRepository : ARepository<ClientTopic>, IClientTopicRepository
    {
        public ClientTopicRepository(DbContextClass context) : base(context)
        {
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<List<Guid>> GetIdClientsByTopicId(Guid topicId)
            => _dbSet.Where(x => x.TopicId == topicId).Select(x => x.ClientId).ToList();
    }
}
