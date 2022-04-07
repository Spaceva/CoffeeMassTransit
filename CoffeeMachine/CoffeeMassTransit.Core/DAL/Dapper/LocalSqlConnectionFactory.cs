namespace CoffeeMassTransit.Core.DAL;

internal class LocalSqlConnectionFactory : SqlConnectionFactory
{
    public LocalSqlConnectionFactory(string connectionString) : base(connectionString) { }
}
