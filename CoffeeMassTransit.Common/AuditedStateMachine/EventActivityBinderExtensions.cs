using MassTransit;
using System;
using System.Collections.Generic;

namespace CoffeeMassTransit.Common.AuditedStateMachine;
public static class EventActivityBinderExtensions
{
    public static EventActivityBinder<TSaga, TData> SaveInput<TSaga, TData, TInput>(this EventActivityBinder<TSaga, TData> binder, Func<TData, TInput> mapToInput)
        where TSaga : AuditedState<TInput>, ISaga
        where TData : class
     => binder.Then(x =>
     {
         x.Saga.Input = mapToInput(x.Message);
     });

    public static EventActivityBinder<TSaga, TData> StartAndTransitionTo<TSaga, TData>(this EventActivityBinder<TSaga, TData> binder, State toState)
        where TSaga : AuditedState, ISaga
        where TData : class
     => binder.Then(x =>
     {
         var now = DateTime.UtcNow;
         x.Saga.StartTime = now;
         x.Saga.History = new List<StateHistoryStep> { new StateHistoryStep { TimeStamp = now, StateAfter = toState.Name, EventName = x.Message.GetType().Name } };
     })
               .TransitionTo(toState);

    public static EventActivityBinder<TSaga, TData> End<TSaga, TData>(this EventActivityBinder<TSaga, TData> binder)
        where TSaga : AuditedState, ISaga
        where TData : class
     => binder.Then(x =>
     {
         var now = DateTime.UtcNow;
         x.Saga.EndTime = now;
         x.Saga.History.Add(new StateHistoryStep { TimeStamp = now, StateAfter = x.StateMachine.Final.Name, EventName = x.Message.GetType().Name });
     })
              .Finalize();

    public static EventActivityBinder<TSaga, TData> SaveAndTransitionTo<TSaga, TData>(this EventActivityBinder<TSaga, TData> binder, State toState)
        where TSaga : AuditedState, ISaga
        where TData : class
     => binder.Then(x =>
     {
         x.Saga.History.Add(new StateHistoryStep { TimeStamp = DateTime.UtcNow, StateAfter = toState.Name, EventName = x.Message.GetType().Name });
     })
              .TransitionTo(toState);

    public static EventActivityBinder<TSaga, TData> Save<TSaga, TData>(this EventActivityBinder<TSaga, TData> binder)
        where TSaga : AuditedState, ISaga
        where TData : class
     => binder.Then(x =>
     {
         x.Saga.History.Add(new StateHistoryStep { TimeStamp = DateTime.UtcNow, EventName = x.Message.GetType().Name });
     });
}
