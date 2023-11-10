using System.Diagnostics.CodeAnalysis;

namespace Models.ViewModel
{
    public class CreateClientViewModel
    {
        [NotNull]
        public string Name { get; set; }
    }
}
