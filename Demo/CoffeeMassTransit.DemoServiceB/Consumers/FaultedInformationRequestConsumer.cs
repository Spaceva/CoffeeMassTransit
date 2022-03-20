using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CoffeeMassTransit.DemoCommon;

namespace CoffeeMassTransit.DemoServiceB;

public class FaultedInformationRequestConsumer : IConsumer<Fault<InformationRequest>>
{
    private readonly ILogger<FaultedInformationRequestConsumer> logger;

    public FaultedInformationRequestConsumer(ILogger<FaultedInformationRequestConsumer> logger)
    {
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<Fault<InformationRequest>> context)
    {
        logger?.LogInformation($"So there was an error.. ok");
        var sendEndpoint = await context.GetSendEndpoint(new Uri("exchange:serviceA"));
        await sendEndpoint.Send<AccessDenied>(new { });
    }
}
