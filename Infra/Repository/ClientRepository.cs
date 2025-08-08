using Infra.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Repository
{
    public class ClientRepository : ARepository<Client>, IClientRepository
    {
        public ClientRepository(DbContextClass context) : base(context)
        {
        }
        public override Client GetById(object id)
        {
            return _dbSet.Include(x => x.Queue).First(x => x.Id == (Guid)id);
        }
        public async Task<List<Client>> GetAllInclude()
        {
            return _dbSet.Include(x => x.Queue).OrderBy(x => x.CreateDate).ToList();
        }
        public async Task<List<Client>> GetAllOnline()
        {
            return _dbSet.Where(x => x.IsOnline == true).Include(x => x.Queue).OrderBy(x => x.CreateDate).ToList();
        }
        public void Dispose() => GC.SuppressFinalize(this);


        public async Task<Client> GetDetailsById(Guid id)
        {
            return _dbSet
                .Include(x => x.MessagesRecevied)
                    .ThenInclude(x => x.ClientSend)
                .Include(x => x.Queue)
                .Include(x => x.Contacts)
                    .ThenInclude(x => x.ClientContact)
                .SingleOrDefault(x => x.Id == id);
        }

        public async Task<List<Client>> GetPossiblesContacts(Guid id)
        {
            var clientePrincipal = _dbSet
             .Include(c => c.Contacts) 
             .SingleOrDefault(c => c.Id == id);

            var idsContatosAtuais = clientePrincipal.Contacts.Select(c => c.ClientContactId);

            return _dbSet
                .Where(c => c.Id != id && !idsContatosAtuais.Contains(c.Id))
                .ToList();
        }
    }
}
