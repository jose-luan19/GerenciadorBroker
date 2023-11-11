using Models.ViewModel;

namespace Services.Interfaces
{
    public interface IMessageService
    {
        Task SaveMessage(string message);
        Task PostMessage(CreateMessageViewModel createMessageViewModel);
    }
}
