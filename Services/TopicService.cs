using AutoMapper;
using CrossCouting;
using Infra.Repository.Interfaces;
using Models;
using Models.ViewModel;
using RabbitMQ.Client;
using Services.Interfaces;

namespace Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IQueueTopicRepository _queueTopicRepository;
        private readonly IQueueRepository _queueRepository;
        private readonly IMapper _mapper;
        private readonly ConfigRabbitMQ _configRabbitMQ;
        public TopicService
            (
            ITopicRepository topicRepository,
            IQueueTopicRepository queueTopicRepository,
            IQueueRepository queueRepository,
            IMapper mapper,
            ConfigRabbitMQ configRabbitMQ
            )
        {
            _topicRepository = topicRepository;
            _queueTopicRepository = queueTopicRepository;
            _queueRepository = queueRepository;
            _mapper = mapper;
            _configRabbitMQ = configRabbitMQ;
        }

        public async Task CreateTopic(CreateTopicViewModel topicViewModel)
        {
            Topic topic = _topicRepository.
                Find(x => x.Name == topicViewModel.Name && x.RoutingKey == topicViewModel.RoutingKey);
            if (topic == null)
            {
                topic = new()
                {
                    Name = topicViewModel.Name,
                    RoutingKey = topicViewModel.RoutingKey,
                };
                _configRabbitMQ.Channel.ExchangeDeclare(exchange: topicViewModel.Name, type: "topic", durable: true, autoDelete: false);
                _topicRepository.Insert(topic);
                _topicRepository.Commit();
            }
        }

        public async Task DeleteTopic(string topicName)
        {
            var topicDelete = _topicRepository.GetAllByFilter(x => x.Name == topicName).AsEnumerable();
            if (topicDelete == null)
            {
                throw new NotFoundException("O Topico não foi encontrado.");
            }
            _configRabbitMQ.Channel.ExchangeDelete(topicName);

            _topicRepository.DeleteRange(topicDelete);
            _topicRepository.Commit();
        }

        public async Task<List<ReadAllTopicsViewModel>> GetAllTopics()
        {
            var queueTopics = await _topicRepository.GetAllInclude();
            var map = _mapper.Map<List<ReadAllTopicsViewModel>>(queueTopics);
            return map;
        }

        public async Task TopicBindQueues(List<CreateQueueViewModel> queues, Topic topic)
        {
            if (queues != null)
            {
                List<Queues> newQueues = new();
                List<QueueTopic> newQueuesTopic = new();
                foreach (var queuesName in queues)
                {
                    var queue = _queueRepository.Find(x => x.Name == queuesName.Name);
                    if (queue != null)
                    {
                        if (!_queueTopicRepository.Any(x => x.QueuesId == queue.Id && x.TopicId == topic.Id))
                        {
                            QueueTopic newQueueTopic = new() { QueuesId = queue.Id, TopicId = topic.Id };
                            newQueuesTopic.Add(newQueueTopic);
                        }
                    }
                    else
                    {
                        queue = new() { Name = queuesName.Name };
                        QueueTopic newQueueTopic = new() { QueuesId = queue.Id, TopicId = topic.Id };
                        newQueues.Add(queue);
                        newQueuesTopic.Add(newQueueTopic);
                        _configRabbitMQ.Channel.QueueDeclare(queue: queue.Name, exclusive: false, durable: true, autoDelete: false);
                    }
                    _configRabbitMQ.Channel.QueueBind(queue: queue.Name, exchange: topic.Name, routingKey: topic.RoutingKey);
                }
                if (newQueues.Count > 0)
                {
                    _queueRepository.InsertRange(newQueues);
                    _queueRepository.Commit();
                }
                if (newQueuesTopic.Count > 0)
                {
                    _queueTopicRepository.InsertRange(newQueuesTopic);
                    _queueTopicRepository.Commit();
                }
            }
        }
    }
}
