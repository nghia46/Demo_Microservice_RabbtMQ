// Payment Service

using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IModel _rabbitMqChannel;
    private readonly ManualResetEvent processingCompleted = new ManualResetEvent(true);

    public PaymentController(IModel rabbitMqChannel)
    {
        _rabbitMqChannel = rabbitMqChannel;

        _rabbitMqChannel.QueueDeclare("order_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_rabbitMqChannel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            ProcessReceivedMessage(message);

            // Signal that processing is completed
            processingCompleted.Set();
        };

        _rabbitMqChannel.BasicConsume("order_queue", autoAck: true, consumer: consumer);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Message sent and processed!");
    }

    private void ProcessReceivedMessage(string message)
    {
            // Your processing logic here
           Console.WriteLine($"Processing message: {message}");
    }
}
