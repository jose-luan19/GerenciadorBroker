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
            ConfigRabbitMQ.Channel.ExchangeDeclare(exchange: topicViewModel.Name, type: "topic");
            if (_topicRepository.ExistByName(topicViewModel.Name))
            {
                throw new AlreadyExistExpection("Topico já existe");
            }
            Topic topic = new()
            {
                Name = topicViewModel.Name,
                RoutingKey = topicViewModel.RoutingKey,
            };

            await TopicBindQueues(topicViewModel.QueuesName, topic);

        }

        public Task DeleteTopic(Guid idTopic)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Topic>> GetAllQueues()
        {
            throw new NotImplementedException();
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
                        QueueTopic newQueueTopic = new() { QueuesId = queue.Id, TopicId = topic.Id };
                        newQueuesTopic.Add(newQueueTopic);
                    }
                    else
                    {
                        Queues newQueue = new() { Name = queuesName.Name };
                        QueueTopic newQueueTopic = new() { QueuesId = newQueue.Id, TopicId = topic.Id };
                        newQueues.Add(newQueue);
                        newQueuesTopic.Add(newQueueTopic);
                    }

                }
                if (newQueues.Count > 0)
                {
                    _queueRepository.InsertRange(newQueues);
                    _queueRepository.Commit();
                }
                _queueTopicRepository.InsertRange(newQueuesTopic);
                _queueTopicRepository.Commit();
            }
            _topicRepository.Insert(topic);
            _topicRepository.Commit();

        }
    }
}
