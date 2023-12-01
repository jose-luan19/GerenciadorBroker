namespace Models
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public Guid QueueId { get; set; }
        public bool IsOnline { get; set; } = true;
        public virtual Queues Queue { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }

    }
}