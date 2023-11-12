namespace Models.ViewModel
{
    public class ReadAllQueueViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public List<string> TopicsNames { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
