using System;

namespace CoffeeMassTransit.Common.AuditedStateMachine;

public class StateHistoryStep
{
    public DateTime TimeStamp { get; set; }

    public string? StateAfter { get; set; }

    public string EventName { get; set; } = default!;
}