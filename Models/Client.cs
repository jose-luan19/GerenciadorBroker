namespace Models
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public List<Message>? Messages { get; set; }
    }
}