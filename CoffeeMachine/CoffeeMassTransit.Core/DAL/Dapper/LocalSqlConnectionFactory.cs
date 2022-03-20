namespace CoffeeMassTransit.Core.DAL;

public class LocalSqlConnectionFactory : SqlConnectionFactory
{
    public LocalSqlConnectionFactory(string connectionString) : base(connectionString) { }
}
