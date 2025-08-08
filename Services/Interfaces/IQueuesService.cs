using Models;
using Models.ViewModel;

namespace Services.Interfaces
{
    public interface IQueueService
    {
        Task<Queues> CreateQueue(CreateQueueViewModel Queues);
        Task DeleteQueue(Queues queue);
    }
}
