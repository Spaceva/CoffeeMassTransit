using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core.DAL;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.Web;

public class PaymentService
{
    private readonly IPaymentRepository paymentRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public PaymentService(IPaymentRepository paymentRepository, IPublishEndpoint publishEndpoint)
    {
        this.paymentRepository = paymentRepository;
        this.publishEndpoint = publishEndpoint;
    }

    public ulong Create(Guid orderId, float amount) => paymentRepository.Create(orderId, amount);
    public IReadOnlyDictionary<Guid, List<Payment>> GetAll() => paymentRepository.GetAllByOrderId() ?? new Dictionary<Guid, List<Payment>>();
    public IReadOnlyCollection<Payment> Get(Guid orderId) => paymentRepository.Get(orderId);
    public Payment GetActive(Guid orderId) => paymentRepository.GetActive(orderId);
    public async Task Pay(Guid orderId, string creditcard, string cc)
    {
        try
        {
            paymentRepository.Pay(orderId, creditcard, cc);
            await publishEndpoint.Publish<PaymentAcceptedEvent>(new { CorrelationId = orderId });
        }
        catch (Exception ex)
        {
            await publishEndpoint.Publish<PaymentRefusedEvent>(new { CorrelationId = orderId, Error = ex.Message });
            throw;
        }
    }
}
