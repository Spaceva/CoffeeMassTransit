using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeMassTransit.DemoCommon
{
    public interface StatusNOKResponse
    {
        string Reason { get; }
    }
}
