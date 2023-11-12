using Infra.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infra.Repository
{
    public class TopicRepository : ARepository<Topic>, ITopicRepository
    {
        public TopicRepository(DbContextClass context) : base(context) { }

        public void Dispose() => GC.SuppressFinalize(this);

        public async Task<List<Topic>> GetAllInclude()
        {
            return _dbSet
                .Include(topic => topic.ClientTopic)
                    .ThenInclude(queueTopic => queueTopic.Client)
                .OrderBy(x => x.CreateDate)
                .ToList();
        }
    }
}
