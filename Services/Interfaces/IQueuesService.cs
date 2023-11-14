using Models;
using Models.ViewModel;

namespace Services.Interfaces
{
    public interface IQueueService
    {
        Task<Queues> CreateQueue(CreateQueueViewModel Queues);
        Task DeleteQueue(Guid idQueue);
        Task<List<ReadAllQueueViewModel>> GetAllQueues();
        Task DeleteQueueAfterClient(Guid idQueue);
    }
}
