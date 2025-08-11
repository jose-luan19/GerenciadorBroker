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
            using var connection = _configRabbitMQ.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queue.Name, durable: true, exclusive: false, autoDelete: false);
            Queues newQueue = new() {Name = queue.Name};
            _repository.Insert(newQueue);
            _repository.Commit();
            return newQueue;
        }

        public async Task DeleteQueue(Queues queue)
        {
            using var connection = _configRabbitMQ.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDelete(queue: queue.Name);
            _repository.Delete(queue);
            _repository.Commit();
        }
    }
}