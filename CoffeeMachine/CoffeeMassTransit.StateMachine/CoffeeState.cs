﻿using Automatonymous;
using Dapper.Contrib.Extensions;
using System;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.StateMachine
{
    public class CoffeeState : SagaStateMachineInstance
    {
        [ExplicitKey]
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string CustomerName { get; set; }
        public string ToppingsRequested { get; set; }
        public CoffeeType CoffeeTypeRequested { get; set; }
        public float Amount { get; set; }
    }
}
