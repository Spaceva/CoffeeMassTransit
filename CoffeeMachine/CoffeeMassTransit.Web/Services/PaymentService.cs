using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeMassTransit.Contracts;
using CoffeeMassTransit.Core.DAL;
using CoffeeMassTransit.Messages;

namespace CoffeeMassTransit.Web
{
    public class PaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public PaymentService(IPaymentRepository paymentRepository, IPublishEndpoint publishEndpoint)
        {
            this.paymentRepository = paymentRepository;
            this.publishEndpoint = publishEndpoint;
        }

        public ulong Create(Guid orderId, float amount) => this.paymentRepository.Create(orderId, amount);
        public IReadOnlyDictionary<Guid, List<Payment>> GetAll() => this.paymentRepository.GetAll() ?? new Dictionary<Guid, List<Payment>>();
        public IReadOnlyCollection<Payment> Get(Guid orderId) => this.paymentRepository.Get(orderId);
        public Payment GetActive(Guid orderId) => this.paymentRepository.GetActive(orderId);
        public async Task Pay(Guid orderId, string creditcard, string cc)
        {
            try
            {
                this.paymentRepository.Pay(orderId, creditcard, cc);
                await this.publishEndpoint.Publish<PaymentAcceptedEvent>(new { CorrelationId = orderId });
            }
            catch (Exception ex)
            {
                await this.publishEndpoint.Publish<PaymentRefusedEvent>(new { CorrelationId = orderId, Error = ex.Message });
                throw;
            }
        }
    }
}
