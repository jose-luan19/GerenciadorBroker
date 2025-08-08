using Models;

namespace Infra.Repository.Interfaces
{
    public interface IContactRepository : IDisposable, IARepository<Contact>
    {
    }
}
