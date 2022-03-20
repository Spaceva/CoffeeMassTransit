namespace CoffeeMassTransit.CoffeeMassTransit.ConsumerSaga;

internal enum CoffeeMachineSagaStates
{
    NotStarted,
    AwaitingPayment,
    Paid,
    BaseCoffeeOK,
    CoffeeReady,
    Ended
}
