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
        public TopicService
            (
            ITopicRepository topicRepository,
            IQueueTopicRepository queueTopicRepository,
            IQueueRepository queueRepository
            )
        {
            _topicRepository = topicRepository;
            _queueTopicRepository = queueTopicRepository;
            _queueRepository = queueRepository;
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
                    RoutingKey= topicViewModel.RoutingKey,
                };
                ConfigRabbitMQ.Channel.ExchangeDeclare(exchange: topicViewModel.Name, type: "topic");
                _topicRepository.Insert(topic);
                _topicRepository.Commit();
            }

            await TopicBindQueues(topicViewModel.QueuesName, topic);

        }

        public async Task DeleteTopic(string topicName)
        {
            var topicDelete = _topicRepository.GetAllByFilter(x => x.Name == topicName).AsEnumerable();
            if (topicDelete == null)
            {
                throw new NotFoundException("O Topico não foi encontrado.");
            }
            ConfigRabbitMQ.Channel.ExchangeDelete(topicName);

            _topicRepository.DeleteRange(topicDelete);
            _queueTopicRepository.DeleteByTopicid(topicDelete);

            

            _topicRepository.Commit();
            _queueTopicRepository.Commit();
        }

        public async Task<IEnumerable<Topic>> GetAllTopics()
        {
            var queueTopics = _queueTopicRepository.Include(i => i.Topic).ToList();
            var queueTopics2 = _queueTopicRepository.Include(i => i.Queues).ToList();
            return _topicRepository.GetAll().AsEnumerable().OrderBy(x => x.CreateDate);
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
                        ConfigRabbitMQ.Channel.QueueDeclare(queue: queue.Name, exclusive: false);
                    }
                    ConfigRabbitMQ.Channel.QueueBind(queue: queue.Name, exchange: topic.Name, routingKey: topic.RoutingKey);

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
