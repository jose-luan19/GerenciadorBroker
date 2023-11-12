using System.Diagnostics.CodeAnalysis;

namespace Models.ViewModel
{
    public class CreateTopicViewModel
    {
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string RoutingKey { get; set; }
    }
}
