using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoffeeMassTransit.EventListener
{
    public abstract class ListenerConsumer<TMessage> : IConsumer<TMessage>
         where TMessage : class, CorrelatedBy<Guid>
    {
        private readonly ILogger<ListenerConsumer<TMessage>> logger;

        public ListenerConsumer(ILogger<ListenerConsumer<TMessage>> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<TMessage> context)
        {
            this.logger?.LogInformation($"I saw a {context.Message.GetType().Name} ! CorrelationId = {context.Message.CorrelationId}");
            return Task.CompletedTask;
        }
    }
}
