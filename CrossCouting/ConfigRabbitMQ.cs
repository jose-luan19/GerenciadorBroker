using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Runtime;

namespace CrossCouting
{
    public class ConfigRabbitMQ
    {
        private readonly RabbitMqSettings _settings;
        private readonly ConnectionFactory _factory;

        public ConfigRabbitMQ(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
            _factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                AutomaticRecoveryEnabled = true, // reconecta sozinho
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10) // tenta reconectar a cada 10s
            };
        }

        public IConnection CreateConnection()
        {
            return _factory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return CreateConnection().CreateModel();
        }
    }


    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}