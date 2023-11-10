﻿using AutoMapper;
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
        public QueueService(IQueueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Queues> CreateQueue(CreateQueueViewModel queue)
        {
            if (_repository.Any(x => x.Name == queue.Name))
            {
                throw new AlreadyExistExpection("Fila já existe");
            }
            ConfigRabbitMQ.Channel.QueueDeclare(queue: queue.Name, durable: true, exclusive: false, autoDelete: false);
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

            Queues queue = _repository.GetById(idQueue);

            if (queue == null)
            {
                throw new NotFoundException("A fila não foi encontrada.");
            }
            ConfigRabbitMQ.Channel.QueueDelete(queue: queue.Name);
            _repository.Delete(queue);
            _repository.Commit();
        }

        public async Task<List<ReadAllQueueViewModel>> GetAllQueues()
        {
            return _mapper.Map<List<ReadAllQueueViewModel>>(await _repository.GetAllInclude());
        }
    }
}