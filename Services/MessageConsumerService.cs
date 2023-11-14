using Infra.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Models.ViewModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace CrossCouting
{
    public class MessageConsumerService : BackgroundService
    {
        private IClientRepository _clientRepository;
        private IMessageService _messageService;
        private readonly IServiceProvider _serviceProvider;
        private List<Client> clientsOn;
        private List<string> consumersTag = new List<string>();

        public MessageConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();
                _messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                clientsOn = await _clientRepository.GetAllOnline();
                var count = 1;
                while (!stoppingToken.IsCancellationRequested)
                {
                    var clientsCurrent = await _clientRepository.GetAllOnline();
                    var consumer = new EventingBasicConsumer(ConfigRabbitMQ.Channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        _messageService.SaveMessage(json);
                    };
                    if(!clientsOn.SequenceEqual(clientsCurrent) || count == 1)
                    {
                        if(consumersTag.Count > 0)
                        {
                            consumersTag.ForEach(client => { ConfigRabbitMQ.Channel.BasicCancel(client); });
                            consumersTag.Clear();
                        }
                        clientsCurrent
                            .ForEach(client => 
                                { 
                                    var consumerTag = ConfigRabbitMQ.Channel.BasicConsume(queue: client.Queue.Name, autoAck: true, consumer: consumer);
                                    consumersTag.Add(consumerTag);
                                }
                            );
                        count++;
                        clientsOn = clientsCurrent;
                    }
                    await Task.Delay(2000, stoppingToken);
                }
            }
        }
    }
}
