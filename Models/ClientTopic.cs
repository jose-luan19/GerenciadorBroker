namespace Models
{
    public class ClientTopic : BaseEntity
    {
        public virtual Client Client { get; set; }
        public virtual Topic Topic { get; set; }
        public Guid ClientId { get; set; }
        public Guid TopicId { get; set; }
    }
}