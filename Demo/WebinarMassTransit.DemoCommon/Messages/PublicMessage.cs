using System;
using System.Collections.Generic;
using System.Text;

namespace WebinarMassTransit.DemoCommon
{
    public interface PublicMessage
    {
        Guid MessageId { get; }
        string Content { get; }
    }
}
