namespace CoffeeMassTransit.Common.AuditedStateMachine;

public abstract class AuditedState<T> : AuditedState
{
    public T? Input { get; set; }
}
