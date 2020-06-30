using MassTransit;
using MassTransit.Testing;
using System;
using System.Linq;
using System.Threading.Tasks;
using CoffeeMassTransit.CoffeeMachine;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core;
using CoffeeMassTransit.Messages;
using Xunit;

namespace CoffeeMassTransit.Tests
{
    public class CoffeeMachineTests
    {
        [Fact]
        public async Task ShouldConsumeMessage()
        {
            using var harness = new InMemoryTestHarness();
            var consumer = harness.Consumer(() => new CreateBaseCoffeeCommandConsumer(new CoffeeInMemoryRepository(), null));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<CreateBaseCoffeeCommand>(new { CorrelationId = Guid.NewGuid(), CoffeeType = CoffeeType.Americano, NoTopping = false });

                Assert.True(await harness.Sent.Any<CreateBaseCoffeeCommand>());

                Assert.True(await harness.Consumed.Any<CreateBaseCoffeeCommand>());

                Assert.True(await consumer.Consumed.Any<CreateBaseCoffeeCommand>());

                Assert.True(await harness.Published.Any<Fault<CreateBaseCoffeeCommand>>() || await harness.Published.Any<BaseCoffeeFinishedEvent>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
