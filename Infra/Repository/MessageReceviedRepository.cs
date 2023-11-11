using Infra.Repository.Interfaces;
using Models;

namespace Infra.Repository
{
    public class MessageReceviedRepository : ARepository<MessageRecevied>, IMessageReceviedRepository
    {
        public MessageReceviedRepository(DbContextClass context) : base(context)
        {
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
