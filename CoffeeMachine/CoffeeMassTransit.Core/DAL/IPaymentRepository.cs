using System;
using System.Collections.Generic;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Core.DAL;

public interface IPaymentRepository
{
    ulong Create(Guid orderId, float amount);
    void Pay(Guid orderId, string creditcard, string cc);
    IReadOnlyDictionary<Guid, List<Payment>> GetAll();
    IReadOnlyCollection<Payment> Get(Guid orderId);
    Payment GetActive(Guid orderId);
}
