namespace Models
{
    public class Message : BaseEntity
    {
        public string Body { get; set; }
        public Guid ClientReceviedId { get; set; }
        public virtual Client ClientRecevied { get; set; }
        public Guid ClientSendId { get; set; }
        public virtual Client ClientSend { get; set; }
        public DateTime? SendMessageDate { get; set; }
    }
}
