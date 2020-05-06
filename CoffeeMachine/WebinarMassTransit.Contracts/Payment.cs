using System;

namespace WebinarMassTransit.Contracts
{
    public class Payment
    {
        public Guid OrderId { get; set; }
        public ulong Id { get; set; }
        public float Amount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsValid { get; set; }
    }
}
