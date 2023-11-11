using Infra.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Repository
{
    public class ClientRepository : ARepository<Client>, IClientRepository
    {
        public ClientRepository(DbContextClass context) : base(context)
        {
        }
        public override Client GetById(object id)
        {
            return _dbSet.Include(x => x.Queue).First(x => x.Id == (Guid)id);
        }
        public async Task<List<Client>> GetAllInclude()
        {
            return _dbSet.Include(x => x.Queue).OrderBy(x => x.CreateDate).ToList();
        }
        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<List<Guid>> GetIdClientsByTopicId(string topicName, string RoutingKey)
         => _dbSet
            .Where(c => c.ClientTopic.Any(ct => ct.Topic.Name == topicName && ct.Topic.RoutingKey == RoutingKey))
            .Select(c => c.Id)
            .ToList();
        
    }
}
