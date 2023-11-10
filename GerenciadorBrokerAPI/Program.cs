using Infra;
using Infra.Repository;
using Infra.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
builder.Services.AddScoped<IARepository<Message>, MessageRepository>();
builder.Services.AddScoped<IARepository<ClientTopic>, ClientTopicRepository>();

builder.Services.AddScoped<IQueueTopicRepository, QueueTopicRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
