// Order Service

// Dependencies
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IModel _rabbitMqChannel;

    public OrderController(IModel rabbitMqChannel)
    {
        _rabbitMqChannel = rabbitMqChannel;

        // Declare the queue if it doesn't exist
        _rabbitMqChannel.QueueDeclare(queue: "order_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    [HttpPost]
    public IActionResult PlaceOrder([FromBody] OrderDto orderDto)
    {
        // Validate and process the order

        // Publish order details to RabbitMQ
        var orderDetails = new { OrderId = Guid.NewGuid(), Product = orderDto.Product, Quantity = orderDto.Quantity };
        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderDetails));

        _rabbitMqChannel.BasicPublish(exchange: "", routingKey: "order_queue", basicProperties: null, body: messageBody);

        return Ok("Order placed successfully!");
    }
}

public class OrderDto
{
    public required string Product { get; set; }
    public int Quantity { get; set; }
}
