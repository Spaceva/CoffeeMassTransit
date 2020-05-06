using MassTransit;
using System;
using WebinarMassTransit.Contracts;
using WebinarMassTransit.Messages;

namespace WebinarMassTransit.Web
{
    public class OrderService
    {
        private readonly IPublishEndpoint publishEndpoint;

        public OrderService(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public Guid Submit(Coffee wantedCoffee, string customerName)
        {
            var newOrderId = Guid.NewGuid();
            publishEndpoint.Publish<OrderSubmittedEvent>(new { CorrelationId = newOrderId, CoffeeType = wantedCoffee.Type, wantedCoffee.Toppings, CustomerName = customerName });
            return newOrderId;
        }
    }
}
