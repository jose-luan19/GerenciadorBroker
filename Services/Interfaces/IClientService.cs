using Models.ViewModel;

namespace Services.Interfaces
{
    public interface IClientService
    {
        Task CreateClient(CreateClientViewModel clientViewModel);
        Task DeleteClient(Guid id);
        Task<ReadClientViewModel> GetClient(Guid id);
        Task<List<ReadClientViewModel>> GetAllClient();
        Task SubscribeTopic(SubscribeTopicViewModel subscribeTopicViewModel);
    }
}
