using System;
using System.Collections.Generic;

namespace CoffeeMassTransit.Common.AuditedStateMachine;
public abstract class AuditedState
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public ICollection<Step> History { get; set; } = Array.Empty<Step>();
}
