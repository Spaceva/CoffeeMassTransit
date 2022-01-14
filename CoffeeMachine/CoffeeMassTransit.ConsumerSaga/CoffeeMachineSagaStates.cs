using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeMassTransit.CoffeeMassTransit.ConsumerSaga
{
    internal enum CoffeeMachineSagaStates
    {
        NotStarted,
        AwaitingPayment,
        Paid,
        BaseCoffeeOK,
        CoffeeReady,
        Ended
    }
}
