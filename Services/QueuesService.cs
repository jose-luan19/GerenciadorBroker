using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ConfigRabbitMQ _configRabbitMQ;

        public QueueService(IQueueRepository repository, IMapper mapper, ConfigRabbitMQ configRabbitMQ)
        {
            _repository = repository;
            _mapper = mapper;
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

        public async Task DeleteQueue(Guid idQueue)
        {

            Queues queue = await _repository.GetByIdIncludeClient(idQueue);

            if (queue == null)
            {
                throw new NotFoundException("A fila não foi encontrada.");
            }
            if(queue.Client != null) 
            {
                throw new DeleteException("Fila não pode ser excluida pois já pertence a um cliente");
            }
            _configRabbitMQ.Channel.QueueDelete(queue: queue.Name);
            _repository.Delete(queue);
            _repository.Commit();
        }

        public async Task DeleteQueueAfterClient(Guid idQueue)
        {
            Queues queue = await _repository.GetByIdIncludeClient(idQueue);
            _configRabbitMQ.Channel.QueueDelete(queue: queue.Name);
            _repository.Delete(queue);
            _repository.Commit();
        }

        public async Task<List<ReadAllQueueViewModel>> GetAllQueues()
        {
            return _mapper.Map<List<ReadAllQueueViewModel>>(await _repository.GetAllInclude());
        }
    }
}