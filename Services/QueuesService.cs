using CrossCouting;
using Infra.Repository;
using Models;
using Models.ViewModel;
using RabbitMQ.Client;
using Services.Interfaces;

namespace Services
{
    public class QueueService : IQueueService
    {
        private readonly IARepository<Queues> _repository;
        public QueueService(IARepository<Queues> repository)
        {
            _repository = repository;
        }

        public async Task CreateQueue(CreateQueueViewModel queue)
        {
            ConfigRabbitMQ.Channel.QueueDeclare(queue: queue.Name, exclusive: false);
            Queues newQueue = new Queues
            {
                Name = queue.Name
            };
            _repository.Insert(newQueue);
            _repository.Commit();
        }

        public async Task DeleteQueue(Guid idQueue)
        {
 
            Queues queue = _repository.GetById(idQueue);
            if(queue == null)
            {
                throw new NotFoundException("A fila não foi encontrado.");
            }
            ConfigRabbitMQ.Channel.QueueDelete(queue: queue.Name);
            _repository.Delete(queue);
            _repository.Commit();
        }
    }
}