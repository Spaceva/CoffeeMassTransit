namespace CoffeeMassTransit.Common
{
    public class AzureServiceBusConfiguration
    {
        public string ConnectionString => $"Endpoint=sb://{Host}/;SharedAccessKeyName={SharedAccessKeyName};SharedAccessKey={SharedAccessKey};";

        public string Host => $"{DomainName}.servicebus.windows.net";

        public string DomainName { get; set; } = default!;

        public string SharedAccessKeyName { get; set; } = default!;

        public string SharedAccessKey { get; set; } = default!;
    }
}
