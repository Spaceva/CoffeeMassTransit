﻿using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CoffeeMassTransit.DemoCommon;

namespace CoffeeMassTransit.DemoServiceB;

public class StatusCheckConsumer : IConsumer<StatusCheck>
{
    private readonly ILogger<StatusCheckConsumer> logger;

    public StatusCheckConsumer(ILogger<StatusCheckConsumer> logger)
    {
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<StatusCheck> context)
    {
        logger?.LogInformation("I'm asked how I feel !");
        if (DateTime.Now.Second % 3 == 0)
        {

            logger?.LogInformation("I'm not so sure...");
            await context.RespondAsync<StatusNOKResponse>(new { Reason = "I'm not so sure..." }); 
            return;
        }

        logger?.LogInformation("It's OK");
        await context.RespondAsync<StatusOKResponse>(new { });
    }
}
