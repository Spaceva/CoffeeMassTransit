using System.Data;
using System.Data.SqlClient;

namespace CoffeeMassTransit.Core.DAL;

internal abstract class SqlConnectionFactory
{
    public SqlConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    private string ConnectionString { get; }

    public IDbConnection Create()
    {
        return new SqlConnection(ConnectionString);
    }
}
