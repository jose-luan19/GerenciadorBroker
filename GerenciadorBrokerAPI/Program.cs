using Infra;
using Infra.Repository;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IARepository<Client>, ClientRepository>();
builder.Services.AddScoped<IARepository<Message>, MessageRepository>();
builder.Services.AddScoped<IARepository<ClientQueue>, ClientQueueRepository>();
builder.Services.AddScoped<IARepository<ClientTopic>, ClientTopicRepository>();
builder.Services.AddScoped<IARepository<QueueTopic>, QueueTopicRepository>();
builder.Services.AddScoped<IARepository<Topic>, TopicRepository>();
builder.Services.AddScoped<IARepository<Queues>, QueueRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MigrationInitialization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
