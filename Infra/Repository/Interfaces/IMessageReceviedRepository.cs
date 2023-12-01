using Models;

namespace Infra.Repository.Interfaces
{
    public interface IMessageReceviedRepository : IDisposable, IARepository<Message>
    {
    }
}
