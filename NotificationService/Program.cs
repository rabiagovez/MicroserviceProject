using Eventing.Events;
using Eventing.Interfaces;
using Eventing.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Configuration;
using NotificationService.Application.Handlers;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Services;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NotificationDb")));

builder.Services.AddTransient<INotificationSender, EmailNotificationSender>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

var configuration = builder.Configuration;
builder.Services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
var rabbitMqOptions = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();

builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = rabbitMqOptions.Host,
        UserName = rabbitMqOptions.User,
        Password = rabbitMqOptions.Password,
        DispatchConsumersAsync = true
    };

    return new DefaultRabbitMQPersistentConnection(factory);
});
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>();
builder.Services.AddTransient<OrderCreatedEventHandler>();

builder.Services.Configure<NotificationSettings>(builder.Configuration.GetSection("NotificationSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>("OrderCreatedEvent_NotificationService");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
