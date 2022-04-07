using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core.DAL;
using MongoDB.Driver;
using System.Text.Json;
using CoffeeMassTransit.Core;

namespace CoffeeMassTransit.DAL.Mongo;

internal class PaymentMongoRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> payments;

    public PaymentMongoRepository(ConnectionFactory<Payment> connectionFactory)
    {
        this.payments = connectionFactory.GetCollection();
    }


    public ulong Create(Guid orderId, float amount)
    {
        var count = (ulong)this.payments.CountDocuments((FilterDefinition<Payment>)"{}");
        payments.InsertOne(new Payment { Id = count + 1, Amount = amount, OrderId = orderId, IsValid = true });
        return count + 1;
    }

    public void Pay(Guid orderId, string creditcard, string cc)
    {
        var jsonFind = (FilterDefinition<Payment>)JsonSerializer.Serialize(new { OrderId = orderId, IsValid = true });

        if (cc == "999")
        {
            FailToPayAndThrow(jsonFind);
        }
        
        var jsonUpdate = (UpdateDefinition<Payment>)JsonSerializer.Serialize(new
        {
            IsPaid = true,
        });

        payments.FindOneAndUpdate(jsonFind, jsonUpdate);
    }

    private void FailToPayAndThrow(FilterDefinition<Payment> jsonFind)
    {
        var jsonUpdateFail = (UpdateDefinition<Payment>)JsonSerializer.Serialize(new
        {
            IsValid = false,
        });

        payments.FindOneAndUpdate(jsonFind, jsonUpdateFail);

        throw new PaymentRefusedException();
    }

    public IReadOnlyCollection<Payment> Get(Guid orderId)
    {
        var jsonFind = (FilterDefinition<Payment>)JsonSerializer.Serialize(new { OrderId = orderId });

        return payments.Find(jsonFind).ToList();
    }

    public Payment GetActive(Guid orderId)
    {
        var jsonFind = (FilterDefinition<Payment>)JsonSerializer.Serialize(new { OrderId = orderId, IsValid = true });

        return payments.Find(jsonFind).FirstOrDefault();
    }

    public IReadOnlyDictionary<Guid, List<Payment>> GetAllByOrderId()
    {
        var payments = this.payments.Find((FilterDefinition<Payment>)"{}").ToList();
        var result = new Dictionary<Guid, List<Payment>>();
        foreach (var payment in payments)
        {
            if (!result.ContainsKey(payment.OrderId))
            {
                result.Add(payment.OrderId, new List<Payment>());
            }

            result[payment.OrderId].Add(payment);
        }
        return result;
    }
}
