using Models;

namespace Infra.Repository
{
    public class ClientRepository : ARepository<Client>
    {
        public ClientRepository(DbContextClass context) : base(context)
        {
        }
    }
}
