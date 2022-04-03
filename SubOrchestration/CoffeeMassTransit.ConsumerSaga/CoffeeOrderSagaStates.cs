namespace CoffeeMassTransit.SubOrchestration.ConsumerSaga;

internal enum CoffeeOrderSagaStates
{
    NotStarted,
    AwaitingPayment,
    Paid,
    BaseCoffeeOK,
    CoffeeReady,
    Ended
}
