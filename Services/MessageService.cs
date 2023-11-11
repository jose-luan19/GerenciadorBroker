﻿using CrossCouting;
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
        public MessageService
        (
        IMessageReceviedRepository messageRepository, 
        IClientRepository clientRepository,
        IClientTopicRepository clientTopicRepository,
        ITopicRepository topicRepository
        )
        {
            _messageRepository = messageRepository;
            _clientRepository = clientRepository;
            _clientTopicRepository = clientTopicRepository;
            _topicRepository = topicRepository;
        }
        public async Task SaveMessage(string json)
        {
            var messageRecevied = JsonSerializer.Deserialize<CreateMessageViewModel>(json);

            if (messageRecevied.ClientId != null)
            {
                MessageRecevied newMessageRecevied = new()
                {
                    Body = messageRecevied.Message,
                    ClientId = (Guid)messageRecevied.ClientId
                };
                _messageRepository.Insert(newMessageRecevied);
            }
            else
            {
                var lisIds = await _clientTopicRepository.GetIdClientsByTopicId((Guid)messageRecevied.TopicId);
                if (!_messageRepository.Any(x => x.Body == messageRecevied.Message && x.ClientId == lisIds.First()))
                {
                    foreach (var clientId in lisIds)
                    {
                        _messageRepository.Insert(
                            new MessageRecevied
                            {
                                Body = messageRecevied.Message,
                                ClientId = clientId,
                            }
                        );
                    }
                }
            }
            _messageRepository.Commit();
        }

        public async Task PostMessage(CreateMessageViewModel createMessageViewModel)
        {
            var json = JsonSerializer.Serialize(createMessageViewModel);
            var body = Encoding.UTF8.GetBytes(json);
            if (createMessageViewModel.ClientId != null)
            {
                var client = _clientRepository.GetById(createMessageViewModel.ClientId);
                ConfigRabbitMQ.Channel
                    .BasicPublish(exchange: "", routingKey: client.Name + ".QUEUE", body: body);
                return;
            }
            var topic = _topicRepository.GetById(createMessageViewModel.TopicId);
            ConfigRabbitMQ.Channel
                .BasicPublish
                (
                    exchange: topic.Name, 
                    routingKey: topic.RoutingKey, 
                    body: body
                );

        }
    }
}
