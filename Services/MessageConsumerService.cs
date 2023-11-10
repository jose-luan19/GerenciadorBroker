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
        private readonly IServiceProvider _serviceProvider;
        private IClientService _clientService;


        private readonly object _lock = new object();
        private string _queueName;

        public void SetQueueName(string queueName)
        {
            if (queueName == null)
            {
                throw new ArgumentNullException(nameof(queueName), "queueName não pode ser nulo");
            }

            lock (_lock)
            {
                _queueName = queueName;
            }
        }

        public MessageConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string currentQueueName;

                lock (_lock)
                {
                    currentQueueName = _queueName;
                }

                if (currentQueueName != null)
                {
                    var consumer = new EventingBasicConsumer(ConfigRabbitMQ.Channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [x] Recebida mensagem: '{message}' com routing key: '{ea.RoutingKey}'");
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            _clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                        }
                    };

                    ConfigRabbitMQ.Channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
