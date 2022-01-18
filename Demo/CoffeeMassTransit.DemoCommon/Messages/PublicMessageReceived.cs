using System;

namespace CoffeeMassTransit.DemoCommon
{
    public interface PublicMessageReceived
    {
        Guid MessageId { get; }
    }
}
