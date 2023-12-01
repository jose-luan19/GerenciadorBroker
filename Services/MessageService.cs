using CrossCouting;
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
        private readonly ConfigRabbitMQ _configRabbitMQ;
        public MessageService
        (
        IMessageReceviedRepository messageRepository, 
        IClientRepository clientRepository,
        IQueueService queueService, 
        ConfigRabbitMQ configRabbitMQ
        )
        {
            _messageRepository = messageRepository;
            _clientRepository = clientRepository;
            _configRabbitMQ = configRabbitMQ;
        }
        public async Task SaveMessage(string json)
        {
            var messageRecevied = JsonSerializer.Deserialize<CreateMessageViewModel>(json);

            Message newMessageRecevied = new()
            {
                Body = messageRecevied.Message,
                ClientId = (Guid)messageRecevied.ClientId,
                SendMessageDate = (DateTime)messageRecevied.SendMessageDate,
            };
            _messageRepository.Insert(newMessageRecevied);
            
            _messageRepository.Commit();
        }

        public async Task PostMessage(CreateMessageViewModel createMessageViewModel)
        {
            createMessageViewModel.SendMessageDate = DateTime.Now;
            var json = JsonSerializer.Serialize(createMessageViewModel);
            var body = Encoding.UTF8.GetBytes(json);
            var client = _clientRepository.GetById(createMessageViewModel.ClientId);
            _configRabbitMQ.Channel
                .BasicPublish(exchange: "", routingKey: client.Queue.Name, body: body);
           
        }
    }
}
