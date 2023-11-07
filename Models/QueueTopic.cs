namespace Models
{
    public class QueueTopic : BaseEntity
    {
        public virtual Queues Queues { get; set; }
        public virtual Topic Topic { get; set; }
        public Guid QueuesId { get; set; }
        public Guid TopicId { get; set; }
    }
}
