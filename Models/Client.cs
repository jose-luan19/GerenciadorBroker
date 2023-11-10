namespace Models
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<MessageRecevied>? Messages { get; set; }
        public Guid QueueId { get; set; }
        public virtual Queues Queue { get; set; }
    }
}