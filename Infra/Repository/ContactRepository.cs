using Infra.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Repository
{
    public class ContactRepository : ARepository<Contact>, IContactRepository
    {
        public ContactRepository(DbContextClass context) : base(context)
        {
        }
        public void Dispose() => GC.SuppressFinalize(this);

    }
}
