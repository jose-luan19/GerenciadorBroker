using Infra.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Repository
{
    public class ClientTopicRepository : ARepository<ClientTopic>, IClientTopicRepository
    {
        public ClientTopicRepository(DbContextClass context) : base(context)
        {
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<List<Client>> GetClientsByTopicId(Guid topicId)
           => _dbSet
            .Include(x => x.Client)
                .ThenInclude(x => x.Messages)
            .Where(x => x.TopicId == topicId && x.Client.IsOnline)
            .Select(x => x.Client)
            .ToList();
        
    }
}
