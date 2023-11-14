using Models.ViewModel;

namespace Services.Interfaces
{
    public interface IMessageService
    {
        Task SaveMessage(string message);
        Task<uint> GetCountMessagesInQueues();
        Task PostMessage(CreateMessageViewModel createMessageViewModel);
    }
}
