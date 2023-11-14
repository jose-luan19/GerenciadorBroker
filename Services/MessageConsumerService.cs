using Infra.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Interfaces;
using System.Text;

namespace CrossCouting
{
    public class MessageConsumerService : BackgroundService
    {
        private IClientRepository _clientRepository;
        private IMessageService _messageService;
        private readonly IServiceProvider _serviceProvider;
        private List<Client> clientsOn;
        private List<string> consumersTag = new List<string>();
        private ConfigRabbitMQ configRabbitMQ;

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
                configRabbitMQ = scope.ServiceProvider.GetRequiredService<ConfigRabbitMQ>();
                clientsOn = await _clientRepository.GetAllOnline();
                var count = 1;
                while (!stoppingToken.IsCancellationRequested)
                {
                    var clientsCurrent = await _clientRepository.GetAllOnline();
                    var consumer = new EventingBasicConsumer(configRabbitMQ.Channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        _messageService.SaveMessage(json);
                    };
                    if ((!clientsOn.SequenceEqual(clientsCurrent) || count == 1) && clientsCurrent.Count > 0)
                    {
                        if (consumersTag.Count > 0)
                        {
                            consumersTag.ForEach(client => { configRabbitMQ.Channel.BasicCancel(client); });
                            consumersTag.Clear();
                        }
                        clientsCurrent
                            .ForEach(client =>
                                {
                                    var consumerTag = configRabbitMQ.Channel.BasicConsume(queue: client.Queue.Name, autoAck: true, consumer: consumer);
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
