using Models.ViewModel;

namespace Services.Interfaces
{
    public interface IClientService
    {
        Task CreateClient(CreateClientViewModel clientViewModel);
        Task AddContact(ContactViewModel bindViewModel);
        Task RemoveContact(ContactViewModel bindViewModel);
        Task DeleteClient(Guid id);
        Task<ReadDetailsClientViewModel> GetClient(Guid id);
        Task<List<ReadAllClientViewModel>> GetAllClient();
        Task<List<ReadAllClientViewModel>> GetPossiblesContactsOfClient(Guid id);
        Task ChangeStatus(Guid id);
    }
}
