using MassTransit;
using MassTransit.Courier.Contracts;

namespace RoutingSlip;

public class CoffeeRoutingSlipEventsConsumer :
    IConsumer<RoutingSlipActivityCompleted>,
    IConsumer<RoutingSlipCompleted>,
    IConsumer<RoutingSlipFaulted>,
    IConsumer<RoutingSlipCompensationFailed>,
    IConsumer<RoutingSlipActivityFaulted>,
    IConsumer<RoutingSlipActivityCompensated>,
    IConsumer<RoutingSlipActivityCompensationFailed>
{
    private readonly ILogger<CoffeeRoutingSlipEventsConsumer> logger;

    public CoffeeRoutingSlipEventsConsumer(ILogger<CoffeeRoutingSlipEventsConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<RoutingSlipActivityCompleted> context)
    {
        this.logger.LogInformation("Activity '{Name}' with TrackingId {Id} finished in {Duration} at {Date}", context.Message.ActivityName, context.Message.TrackingNumber, context.Message.Duration, context.Message.Timestamp);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<RoutingSlipCompleted> context)
    {
        this.logger.LogInformation("Orchestration with TrackingId {Id} completed after {Duration} at {Date}", context.Message.TrackingNumber, context.Message.Duration, context.Message.Timestamp);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
    {
        this.logger.LogInformation("Orchestration with TrackingId {Id} failed after {Duration} at {Date}", context.Message.TrackingNumber, context.Message.Duration, context.Message.Timestamp);
        foreach (var activityError in context.Message.ActivityExceptions)
        {
            this.logger.LogError("Error with Activity {Name} at {Date} with Message {Message}", activityError.Name, activityError.Timestamp, activityError.ExceptionInfo.Message);
        }
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<RoutingSlipCompensationFailed> context)
    {
        this.logger.LogInformation("Orchestration with TrackingId {Id} failed to compensate after {Duration} at {Date} because {Error}", context.Message.TrackingNumber, context.Message.Duration, context.Message.Timestamp, context.Message.ExceptionInfo.Message);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<RoutingSlipActivityFaulted> context)
    {
        this.logger.LogInformation("Activity '{Name}' with TrackingId {Id} faulted in {Duration} at {Date} because {Error}", context.Message.ActivityName, context.Message.TrackingNumber, context.Message.Duration, context.Message.Timestamp, context.Message.ExceptionInfo.Message);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<RoutingSlipActivityCompensated> context)
    {
        this.logger.LogInformation("Activity '{Name}' with TrackingId {Id} compensated in {Duration} at {Date}", context.Message.ActivityName, context.Message.TrackingNumber, context.Message.Duration, context.Message.Timestamp);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<RoutingSlipActivityCompensationFailed> context)
    {
        this.logger.LogInformation("Activity '{Name}' with TrackingId {Id} compensatation failed in {Duration} at {Date} because {Error}", context.Message.ActivityName, context.Message.TrackingNumber, context.Message.Duration, context.Message.Timestamp, context.Message.ExceptionInfo.Message);
        return Task.CompletedTask;
    }
}
