using System;
using System.Collections.Generic;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Core.DAL;

public class PaymentInMemoryRepository : IPaymentRepository
{
    private readonly Dictionary<Guid, List<Payment>> PaymentsByOrderId = new();
    private readonly Dictionary<Guid, Payment> ActivePaymentByOrderId = new();
    private ulong currentIndex = 0;

    public ulong Create(Guid orderId, float amount)
    {
        var isNew = false;
        if (!PaymentsByOrderId.ContainsKey(orderId))
        {
            PaymentsByOrderId.Add(orderId, new List<Payment>());
            isNew = true;
        }
        var payment = new Payment { Id = ++currentIndex, Amount = amount, OrderId = orderId, IsValid = true };
        PaymentsByOrderId[orderId].Add(payment);
        if (isNew)
            ActivePaymentByOrderId.Add(orderId, payment);
        else
            ActivePaymentByOrderId[orderId] = payment;
        return currentIndex;
    }
    public void Pay(Guid orderId, string creditcard, string cc)
    {
        var payment = GetActive(orderId);
        if (cc == "999")
        {
            payment.IsValid = false;
            throw new PaymentRefusedException();
        }
        payment.IsPaid = true;
        payment.IsValid = true;
    }

    public IReadOnlyCollection<Payment> Get(Guid orderId)
    {
        if (!PaymentsByOrderId.TryGetValue(orderId, out List<Payment>? payment))
            throw new ArgumentOutOfRangeException(nameof(orderId));
        return payment.ToArray();
    }

    public Payment GetActive(Guid orderId)
    {
        if (!ActivePaymentByOrderId.TryGetValue(orderId, out Payment? payment))
            throw new ArgumentOutOfRangeException(nameof(orderId));
        return payment;
    }

    public IReadOnlyDictionary<Guid, List<Payment>> GetAll()
    {
        return PaymentsByOrderId;
    }
}
