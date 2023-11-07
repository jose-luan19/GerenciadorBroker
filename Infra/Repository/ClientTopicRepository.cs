using Models;

namespace Infra.Repository
{
    public class ClientTopicRepository : ARepository<ClientTopic>
    {
        public ClientTopicRepository(DbContextClass context) : base(context)
        {
        }
    }
}
