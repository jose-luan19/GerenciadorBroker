using Models;

namespace Infra.Repository
{
    public class ClientQueueRepository : ARepository<ClientQueue>
    {
        public ClientQueueRepository(DbContextClass context) : base(context)
        {
        }
    }
}
