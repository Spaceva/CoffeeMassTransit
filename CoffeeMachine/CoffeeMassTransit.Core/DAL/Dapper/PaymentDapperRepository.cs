using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeMassTransit.Contracts;

namespace CoffeeMassTransit.Core.DAL;

internal class PaymentDapperRepository : IPaymentRepository
{
    private readonly SqlConnectionFactory sqlConnectionFactory;

    public PaymentDapperRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        this.sqlConnectionFactory = sqlConnectionFactory;
    }


    public ulong Create(Guid orderId, float amount)
    {
        var query = @$"INSERT INTO   [Payments] ({nameof(Payment.OrderId)}, {nameof(Payment.Amount)}, {nameof(Payment.IsValid)}) 
                        OUTPUT          Inserted.Id
                        VALUES          (@{nameof(orderId)}, @{nameof(amount)}, 1)";
        using var conn = sqlConnectionFactory.Create();
        return conn.ExecuteScalar<ulong>(query, new { orderId, amount });
    }

    public void Pay(Guid orderId, string creditcard, string cc)
    {
        if (cc == "999")
        {
            FailToPay(orderId);
        }

        var query = @$"UPDATE   [Payments] SET {nameof(Payment.IsPaid)}  = 1 WHERE {nameof(Payment.OrderId)} = @{nameof(orderId)} AND {nameof(Payment.IsValid)} = 1";
        using var conn = sqlConnectionFactory.Create();
        conn.Execute(query, new { orderId });
    }

    private void FailToPay(Guid orderId)
    {
        var query = @$"UPDATE   [Payments] SET {nameof(Payment.IsValid)}  = 0 WHERE {nameof(Payment.OrderId)} = @{nameof(orderId)} AND {nameof(Payment.IsValid)} = 1";
        using var conn = sqlConnectionFactory.Create();
        conn.Execute(query, new { orderId });
        throw new PaymentRefusedException();
    }

    public IReadOnlyCollection<Payment> Get(Guid orderId)
    {
        var query = $"SELECT * FROM [Payments] WHERE [{nameof(Payment.OrderId)}] = @{nameof(orderId)}";
        using var conn = sqlConnectionFactory.Create();
        return conn.Query<Payment>(query, new { orderId }).ToArray();
    }

    public Payment GetActive(Guid orderId)
    {
        var query = $"SELECT * FROM [Payments] WHERE [{nameof(Payment.OrderId)}] = @{nameof(orderId)} AND {nameof(Payment.IsValid)} = 1";
        using var conn = sqlConnectionFactory.Create();
        return conn.QuerySingleOrDefault<Payment>(query, new { orderId });
    }

    public IReadOnlyDictionary<Guid, List<Payment>> GetAllByOrderId()
    {
        var query = $"SELECT * FROM [Payments]";
        using var conn = sqlConnectionFactory.Create();
        var payments = conn.Query<Payment>(query);
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
