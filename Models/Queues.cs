
namespace Models
{
    public class Queues : BaseEntity
    {
        public string Name { get; set; }
        //public Guid? ClientId { get; set; }
        public virtual Client? Client { get; set; }
    }
}