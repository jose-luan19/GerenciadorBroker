using CrossCouting;
using Infra;
using Infra.Repository;
using Infra.Repository.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Carregar appsettings + variáveis de ambiente
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

// Add services to the container.
builder.Services.AddSingleton<ConfigRabbitMQ>();

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<MessageConsumerService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MessageConsumerService>());
string a = builder.Configuration["ServerFront"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    options.AddPolicy("AllowAngularApp",
        b =>
        {
            b.WithOrigins("http://" + builder.Configuration["ServerFront"] + ":4200")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
        });
});

var app = builder.Build();

/*if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    
}*/

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngularApp");
app.UseCors("AllowAny");

app.MigrationInitialization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
