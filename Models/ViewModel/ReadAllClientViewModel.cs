namespace Models.ViewModel
{
    public class ReadAllClientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        //public List<Message> Messages { get; set; }
        //public string QueueName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
