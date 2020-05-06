using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WebinarMassTransit.Common
{
    public class BusControlService : IHostedService
    {
        private readonly IBusControl busControl;

        public BusControlService(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.busControl.StopAsync(cancellationToken);
        }
    }
}
