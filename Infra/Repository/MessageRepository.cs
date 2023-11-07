using Models;

namespace Infra.Repository
{
    public class MessageRepository : ARepository<Message>
    {
        public MessageRepository(DbContextClass context) : base(context)
        {
        }
    }
}
