namespace Models
{
    public class Message : BaseEntity
    {
        public string Body { get; set; }
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
