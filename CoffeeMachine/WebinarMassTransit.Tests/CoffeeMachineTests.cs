using MassTransit;
using MassTransit.Testing;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebinarMassTransit.CoffeeMachine;
using WebinarMassTransit.Contracts;
using WebinarMassTransit.Core;
using WebinarMassTransit.Messages;
using Xunit;

namespace WebinarMassTransit.Tests
{
    public class CoffeeMachineTests
    {
        [Fact]
        public async Task ShouldConsumeMessage()
        {
            var harness = new InMemoryTestHarness();
            var consumer = harness.Consumer(() => new CreateBaseCoffeeCommandConsumer(new CoffeeInMemoryRepository(), null));

            await harness.Start();
            try
            {
                await harness.InputQueueSendEndpoint.Send<CreateBaseCoffeeCommand>(new { CorrelationId = Guid.NewGuid(), CoffeeType = CoffeeType.Americano, NoTopping = false });

                Assert.True(harness.Consumed.Select<CreateBaseCoffeeCommand>().Any());

                Assert.True(consumer.Consumed.Select<CreateBaseCoffeeCommand>().Any());

                Assert.Contains(harness.Published, publishedMsg => publishedMsg.MessageType == typeof(Fault<CreateBaseCoffeeCommand>) || publishedMsg.MessageType == typeof(BaseCoffeeFinishedEvent));
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
