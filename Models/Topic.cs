namespace Models
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public virtual ICollection<QueueTopic> QueueTopics { get; set; }
        public virtual ICollection<ClientTopic> ClientTopic { get; set; }

    }
}