namespace Models
{
    public class Contact : BaseEntity
    {
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }
        public Guid ClientContactId { get; set; }
        public virtual Client ClientContact { get; set; }
    }
}
