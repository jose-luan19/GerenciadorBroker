namespace Models.ViewModel
{
    public class ReadAllTopicsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public ICollection<ReadQueueViewModel> Queues { get; set; }
        //public DateTime CreateDate { get; set; }
    }
}
