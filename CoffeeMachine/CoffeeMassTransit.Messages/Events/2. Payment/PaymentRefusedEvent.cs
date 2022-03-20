using MassTransit;
using System;

namespace CoffeeMassTransit.Messages;

public interface PaymentAcceptedEvent : CorrelatedBy<Guid>
{
}
