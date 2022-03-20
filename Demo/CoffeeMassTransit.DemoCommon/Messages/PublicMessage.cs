using System;

namespace CoffeeMassTransit.DemoCommon;

public interface PublicMessage
{
    Guid MessageId { get; }
    string Content { get; }
}
