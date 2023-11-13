namespace Models.ViewModel
{
    public class ReadTopicViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
