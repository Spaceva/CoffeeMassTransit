namespace CoffeeMassTransit.Common;

public class RabbitMQConfiguration
{
    public string Uri => $"rabbitmq://{Host}/{VirtualHost}";
    public string Host { get; init; } = default!;
    public string VirtualHost { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}
