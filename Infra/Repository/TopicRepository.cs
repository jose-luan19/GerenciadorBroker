using Infra.Repository.Interfaces;
using Models;

namespace Infra.Repository
{
    public class TopicRepository : ARepository<Topic>, ITopicRepository
    {
        public TopicRepository(DbContextClass context) : base(context){}
        public bool ExistByName(string name)
        {
            return _dbSet.Any(x => x.Name == name);
        }
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
