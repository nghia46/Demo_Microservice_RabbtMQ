using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(x => new ConnectionFactory() { HostName = "localhost" }.CreateConnection());
builder.Services.AddSingleton(x => x.GetRequiredService<IConnection>().CreateModel());
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
