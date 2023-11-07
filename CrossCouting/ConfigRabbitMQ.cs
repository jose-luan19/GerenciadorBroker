using RabbitMQ.Client;

namespace CrossCouting
{
    public static class ConfigRabbitMQ
    {
        private static readonly ConnectionFactory factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
        private static readonly IConnection connection = factory.CreateConnection();

        public static IModel Channel { get; } = connection.CreateModel();
    }
}