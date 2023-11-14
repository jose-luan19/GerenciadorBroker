using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace CrossCouting
{
    public class ConfigRabbitMQ
    {
        private readonly IConfiguration _configuration;
        private readonly string _host;
        public ConfigRabbitMQ(IConfiguration configuration)
        {
            _configuration = configuration;
            _host = _configuration["ServerRabbitMQ"];
            configure();
        }

        private ConnectionFactory factory;
        private IConnection connection;
        public IModel Channel { get; private set; } 

        private void configure()
        {
            factory = new ConnectionFactory
            {
                HostName = _host,
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            connection = factory.CreateConnection();
            Channel = connection.CreateModel();
        }

    }
}