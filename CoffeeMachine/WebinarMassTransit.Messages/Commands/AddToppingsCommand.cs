using MassTransit;
using System;
using WebinarMassTransit.Contracts;

namespace WebinarMassTransit.Messages
{
    public interface AddToppingsCommand : CorrelatedBy<Guid>
    {
        Topping[] Toppings { get; }
    }
}
