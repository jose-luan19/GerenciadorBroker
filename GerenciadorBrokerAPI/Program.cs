using CrossCouting;
using Infra;
using Infra.Repository;
using Infra.Repository.Interfaces;
using Models;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientTopicRepository, ClientTopicRepository>();
builder.Services.AddScoped<IQueueTopicRepository, QueueTopicRepository>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<MessageConsumerService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MessageConsumerService>());

var app = builder.Build();

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
