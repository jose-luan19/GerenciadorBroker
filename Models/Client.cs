namespace Models
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; } = true;
        public Guid QueueId { get; set; }
        public virtual Queues Queue { get; set; }
        public virtual ICollection<Message>? MessagesRecevied { get; set; }
        public virtual ICollection<Contact>? Contacts { get; set; }
    }
}