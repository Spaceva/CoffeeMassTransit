using MassTransit;
using MassTransit.Testing;
using System;
using System.Threading.Tasks;
using CoffeeMassTransit.CoffeeMachine;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using CoffeeMassTransit.Core.DAL;

namespace CoffeeMassTransit.Tests;

public class CoffeeMachineTests
{
    [Fact]
    public async Task ShouldConsumeMessage()
    {
        await using var provider = new ServiceCollection()
            .AddScoped<ICoffeeRepository, CoffeeInMemoryRepository>()
            .AddMassTransitTestHarness(cfgHarness =>
            {
                cfgHarness.AddConsumer<CreateBaseCoffeeCommandConsumer>();
            })
            .BuildServiceProvider(true);

        var harness = provider.GetTestHarness();
        await harness.Start();

        var consumer = harness.GetConsumerHarness<CreateBaseCoffeeCommandConsumer>();
        var sendEndpoint = await harness.GetConsumerEndpoint<CreateBaseCoffeeCommandConsumer>();


        await sendEndpoint.Send<CreateBaseCoffeeCommand>(new { CorrelationId = Guid.NewGuid(), CoffeeType = CoffeeType.Americano, NoTopping = false });

        Assert.True(await harness.Sent.Any<CreateBaseCoffeeCommand>());

        Assert.True(await consumer.Consumed.Any<CreateBaseCoffeeCommand>());

        Assert.True(await harness.Published.Any<Fault<CreateBaseCoffeeCommand>>() || await harness.Published.Any<BaseCoffeeFinishedEvent>());
    }
}
