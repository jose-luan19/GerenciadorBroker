namespace Models.ViewModel
{
    public class ReadAllTopicsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? RoutingKey { get; set; }
        public List<Queues>? Queues { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
