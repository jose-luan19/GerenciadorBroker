namespace Models.ViewModel
{
    public class ReadDetailsClientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string QueueName { get; set; }
        public ICollection<ReadMessageViewModel> Messages { get; set; }
        public ICollection<ReadTopicViewModel> Topics { get; set; }

    }
}
