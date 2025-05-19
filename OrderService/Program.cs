using Eventing.Interfaces;
using Eventing.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Handlers;
using OrderService.Application.Interfaces;
using OrderService.Application.Services;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Repositories;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<OutboxPublisherService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
//deneme

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDb")));

builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly));

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
