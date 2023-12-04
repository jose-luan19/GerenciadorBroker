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

        public DbSet<Client> Clients { get; set; }
        public DbSet<Queues> Queues { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(c => c.MessagesRecevied)
                .WithOne(m => m.ClientRecevied)
                .HasForeignKey(m => m.ClientReceviedId);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Contacts)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
