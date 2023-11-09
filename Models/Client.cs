namespace Models
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Message>? Messages { get; set; }
        public Guid QueueId { get; set; }
        public virtual Queues Queue { get; set; }
    }
}