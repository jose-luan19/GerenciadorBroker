using System.Diagnostics.CodeAnalysis;

namespace Models.ViewModel
{
    public class CreateMessageViewModel
    {
        public Guid? ClientId { get; set; }
        [NotNull]
        public string Message { get; set; }
        public Guid? TopicId { get; set; }
    }
}
