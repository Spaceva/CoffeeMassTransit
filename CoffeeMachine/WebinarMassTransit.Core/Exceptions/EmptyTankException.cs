using System;
using System.Runtime.Serialization;

namespace WebinarMassTransit.Core
{
    [Serializable]
    public class EmptyTankException : Exception
    {
        public EmptyTankException()
        {
        }

        public EmptyTankException(string message) : base(message)
        {
        }

        public EmptyTankException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyTankException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}