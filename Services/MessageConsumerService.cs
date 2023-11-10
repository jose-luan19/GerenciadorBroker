using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Interfaces;
using System.Text;

namespace CrossCouting
{
    public class MessageConsumerService : BackgroundService
    {
        private IClientService _clientService;
        private readonly IServiceProvider _serviceProvider;

        public MessageConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    var clients = await _clientService.GetAllClient();
                    foreach (var client in clients)
                    {
                        var consumer = new EventingBasicConsumer(ConfigRabbitMQ.Channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine($" [x] Recebida mensagem: '{message}' com routing key: '{ea.RoutingKey}'");
                            
                        };
                        ConfigRabbitMQ.Channel.BasicConsume(queue: client.Queue.Name, autoAck: true, consumer: consumer);
                    }
                    await Task.Delay(2000, stoppingToken);
                }
            }
        }
    }
}
