namespace Models
{
    public class MessageRecevied : BaseEntity
    {
        public string Body { get; set; }
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }
        public DateTime SendMessageDate { get; set; }
    }
}
