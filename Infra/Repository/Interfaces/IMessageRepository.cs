using Models;

namespace Infra.Repository.Interfaces
{
    public interface IMessageRepository : IDisposable, IARepository<MessageRecevied>
    {
    }
}
