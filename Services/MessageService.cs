using CrossCouting;
using Infra.Repository;
using Infra.Repository.Interfaces;
using Models;
using Models.ViewModel;
using RabbitMQ.Client;
using Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageReceviedRepository _messageRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IClientTopicRepository _clientTopicRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IQueueService _queueService;
        private readonly ConfigRabbitMQ _configRabbitMQ;
        public MessageService
        (
        IMessageReceviedRepository messageRepository, 
        IClientRepository clientRepository,
        IClientTopicRepository clientTopicRepository,
        ITopicRepository topicRepository,
        IQueueService queueService, 
        ConfigRabbitMQ configRabbitMQ
        )
        {
            _messageRepository = messageRepository;
            _clientRepository = clientRepository;
            _clientTopicRepository = clientTopicRepository;
            _topicRepository = topicRepository;
            _queueService = queueService;
            _configRabbitMQ = configRabbitMQ;
        }
        public async Task SaveMessage(string json)
        {
            var messageRecevied = JsonSerializer.Deserialize<CreateMessageViewModel>(json);

            if (messageRecevied.ClientId != null)
            {
                MessageRecevied newMessageRecevied = new()
                {
                    Body = messageRecevied.Message,
                    ClientId = (Guid)messageRecevied.ClientId,
                    SendMessageDate = (DateTime)messageRecevied.SendMessageDate,
                };
                _messageRepository.Insert(newMessageRecevied);
            }
            else
            {
                var lisIds = await _clientTopicRepository.GetClientsByTopicId((Guid)messageRecevied.TopicId);
                foreach (var client in lisIds)
                {
                    if(!client.Messages.Any(x => x.SendMessageDate == messageRecevied.SendMessageDate))
                    {
                        _messageRepository.Insert(
                            new MessageRecevied
                            {
                                Body = messageRecevied.Message,
                                ClientId = client.Id,
                                SendMessageDate = (DateTime)messageRecevied.SendMessageDate
                            }
                        );
                    }
                }
            }
            _messageRepository.Commit();
        }

        public async Task PostMessage(CreateMessageViewModel createMessageViewModel)
        {
            createMessageViewModel.SendMessageDate = DateTime.Now;
            var json = JsonSerializer.Serialize(createMessageViewModel);
            var body = Encoding.UTF8.GetBytes(json);
            if (createMessageViewModel.ClientId != null)
            {
                var client = _clientRepository.GetById(createMessageViewModel.ClientId);
                _configRabbitMQ.Channel
                    .BasicPublish(exchange: "", routingKey: client.Queue.Name, body: body);
                return;
            }
            var topic = _topicRepository.GetById(createMessageViewModel.TopicId);
            _configRabbitMQ.Channel
                .BasicPublish
                (
                    exchange: topic.Name, 
                    routingKey: topic.RoutingKey, 
                    body: body
                );
        }

        public async Task<uint> GetCountMessagesInQueues()
        {
            uint count = 0;
            var queues = await _queueService.GetAllQueues();
            queues.ForEach(q => { count += _configRabbitMQ.Channel.MessageCount(q.Name); });
            return count;
        }
    }
}
