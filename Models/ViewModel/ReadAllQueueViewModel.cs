namespace Models.ViewModel
{
    public class ReadAllQueueViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ReadClientQueueViewModel Client { get; set; }
    }
}
