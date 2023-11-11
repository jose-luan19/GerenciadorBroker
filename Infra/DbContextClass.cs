using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace Infra
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string? connectionString = Configuration.GetConnectionString("MySQlConnectionString");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Queues> Queues { get; set; }
        public DbSet<ClientTopic> ClientTopic { get; set; }
        //public DbSet<ClientQueue> ClientQueue { get; set; }
        public DbSet<QueueTopic> QueueTopic { get; set; }
        public DbSet<MessageRecevied> MessageRecevied { get; set; }


    }
}
