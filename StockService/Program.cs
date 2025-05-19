using Eventing.Events;
using Eventing.Interfaces;
using Eventing.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using StockService.Application.Consumers;
using StockService.Application.Handlers;
using StockService.Application.Interfaces;
using StockService.Infrastructure;
using StockService.Infrastructure.Repositories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("StockDb")));


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetStockByProductIdHandler).Assembly));

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
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>();
builder.Services.AddTransient<OrderCreatedEventHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<StockDbContext>();
//    db.Database.Migrate();
//}

var eventBus = app.Services.GetRequiredService<IEventBus>();
//eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();
eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>("OrderCreatedEvent_StockService");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StockDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
