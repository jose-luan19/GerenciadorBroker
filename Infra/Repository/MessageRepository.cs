using Infra.Repository.Interfaces;
using Models;

namespace Infra.Repository
{
    public class MessageRepository : ARepository<Message>, IMessageRepository
    {
        public MessageRepository(DbContextClass context) : base(context)
        {
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
