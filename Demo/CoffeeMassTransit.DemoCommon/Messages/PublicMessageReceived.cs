﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeMassTransit.DemoCommon
{
    public interface PublicMessageReceived
    {
        Guid MessageId { get; }
    }
}
