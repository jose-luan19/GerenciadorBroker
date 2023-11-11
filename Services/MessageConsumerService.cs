using Infra.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                while (!stoppingToken.IsCancellationRequested)
                {
                    var clients = await _clientRepository.GetAllInclude();
                    foreach (var client in clients)
                    {
                        var consumer = new EventingBasicConsumer(ConfigRabbitMQ.Channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var json = Encoding.UTF8.GetString(body);
                            _messageService.SaveMessage(json);
                        };
                        ConfigRabbitMQ.Channel.BasicConsume(queue: client.Queue.Name, autoAck: true, consumer: consumer);
                    }
                    await Task.Delay(2000, stoppingToken);
                }
            }
        }
    }
}
