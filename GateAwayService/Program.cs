using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 🔸 Ocelot yapılandırma dosyasını ekle
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔸 Ocelot servislerini ekle
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//cvbnmöçvcxnmöfcxnm
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 🔸 Ocelot middleware'i çalıştır
await app.UseOcelot();

app.Run();
