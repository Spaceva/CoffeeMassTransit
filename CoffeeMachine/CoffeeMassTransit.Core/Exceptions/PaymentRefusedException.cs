using System;
using System.Runtime.Serialization;

namespace CoffeeMassTransit.Core;

[Serializable]
public class PaymentRefusedException : Exception
{
    public PaymentRefusedException()
    {
    }

    public PaymentRefusedException(string message) : base(message)
    {
    }

    public PaymentRefusedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PaymentRefusedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
