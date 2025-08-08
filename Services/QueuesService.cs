using CrossCouting;
using Infra.Repository.Interfaces;
using Models;
using Models.ViewModel;
using RabbitMQ.Client;
using Services.Interfaces;

namespace Services
{
    public class QueueService : IQueueService
    {
        private readonly IQueueRepository _repository;
        private readonly ConfigRabbitMQ _configRabbitMQ;

        public QueueService(IQueueRepository repository, ConfigRabbitMQ configRabbitMQ)
        {
            _repository = repository;
            _configRabbitMQ = configRabbitMQ;
        }

        public async Task<Queues> CreateQueue(CreateQueueViewModel queue)
        {
            if (_repository.Any(x => x.Name == queue.Name))
            {
                throw new AlreadyExistExpection("Fila já existe");
            }
            _configRabbitMQ.Channel.QueueDeclare(queue: queue.Name, durable: true, exclusive: false, autoDelete: false);
            Queues newQueue = new Queues
            {
                Name = queue.Name
            };
            _repository.Insert(newQueue);
            _repository.Commit();
            return newQueue;
        }

        public async Task DeleteQueue(Queues queue)
        {
            _configRabbitMQ.Channel.QueueDelete(queue: queue.Name);
            _repository.Delete(queue);
            _repository.Commit();
        }
    }
}