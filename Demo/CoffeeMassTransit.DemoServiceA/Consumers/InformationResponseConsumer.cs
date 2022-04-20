using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CoffeeMassTransit.DemoCommon;

namespace CoffeeMassTransit.DemoServiceA;

public class InformationResponseConsumer : IConsumer<InformationResponse>, IConsumer<Fault<InformationRequest>>
{
    private readonly ILogger<InformationResponseConsumer> logger;

    public InformationResponseConsumer(ILogger<InformationResponseConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<InformationResponse> context)
    {
        logger?.LogInformation("Received the information !");
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<InformationRequest>> context)
    {
        logger?.LogInformation("Information was NOT provided");
        return Task.CompletedTask;
    }
}
