using Models;

namespace Infra.Repository
{
    public class TopicRepository : ARepository<Topic>
    {
        public TopicRepository(DbContextClass context) : base(context)
        {
        }
    }
}
