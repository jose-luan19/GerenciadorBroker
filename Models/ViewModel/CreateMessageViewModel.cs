namespace Models.ViewModel
{
    public class CreateMessageViewModel
    {
        public Guid ClientSendId { get; set; }
        public Guid ClientReceviedId { get; set; }
        public string Message { get; set; }
        public DateTime? SendMessageDate { get; set; }
    }
}
