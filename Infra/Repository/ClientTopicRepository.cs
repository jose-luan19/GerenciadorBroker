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

    }
}
