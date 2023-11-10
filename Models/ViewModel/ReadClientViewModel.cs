namespace Models.ViewModel
{
    public class ReadClientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MessageRecevied> Messages { get; set; }
        public ReadQueueViewModel Queue { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
