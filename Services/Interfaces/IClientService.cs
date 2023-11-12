using Models.ViewModel;

namespace Services.Interfaces
{
    public interface IClientService
    {
        Task CreateClient(CreateClientViewModel clientViewModel);
        Task DeleteClient(Guid id);
        Task<ReadAllClientViewModel> GetClient(Guid id);
        Task<List<ReadAllClientViewModel>> GetAllClient();
        Task SubscribeTopic(SubscribeTopicViewModel subscribeTopicViewModel);
    }
}
