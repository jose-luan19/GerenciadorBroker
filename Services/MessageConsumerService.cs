using Infra.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models;
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
        private List<Client> clientsCurrent;
        private List<string> consumersTag = new List<string>();
        private ConfigRabbitMQ _configRabbitMQ;
        private IConfiguration _configuration;
        private RabbitMqSettings _settings;



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
                _configRabbitMQ = scope.ServiceProvider.GetRequiredService<ConfigRabbitMQ>();
                _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                _settings = scope.ServiceProvider.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                clientsOn = await _clientRepository.GetAllOnline();

                using var connection = _configRabbitMQ.CreateConnection();
                using var channel = connection.CreateModel();

                var count = 1;
                while (!stoppingToken.IsCancellationRequested)
                {
                    clientsCurrent = await _clientRepository.GetAllOnline();

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        _messageService.SaveMessage(json);
                    };
                    if ((!clientsOn.SequenceEqual(clientsCurrent) || count == 1) && clientsCurrent.Count > 0)
                    {
                        if (count > 1)
                        {
                            var httpClient = new HttpClient();
                            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_settings.UserName}:{_settings.Password}"))}");
                            var response = await httpClient.GetStringAsync($"http://{_settings.Host}:15672/api/consumers");
                            var consumers = JsonSerializer.Deserialize<List<Consumer>>(response);
                            consumers.ForEach(consumer => { channel.BasicCancel(consumer.ConsumerTag); });
                        }
                        clientsCurrent
                            .ForEach(client =>
                                {
                                    channel.BasicConsume(queue: client.Queue.Name, autoAck: true, consumer: consumer);
                                }
                            );
                        count++;
                        clientsOn = clientsCurrent;
                    }
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}
