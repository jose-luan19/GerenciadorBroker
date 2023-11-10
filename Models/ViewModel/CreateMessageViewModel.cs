using System.Diagnostics.CodeAnalysis;

namespace Models.ViewModel
{
    public class CreateMessageViewModel
    {
        [NotNull]
        public Guid ClientId { get; set; }
        [NotNull]
        public string Message { get; set; }
        public string? Topic { get; set; }
        public string? RoutingKey { get; set; }
    }
}
