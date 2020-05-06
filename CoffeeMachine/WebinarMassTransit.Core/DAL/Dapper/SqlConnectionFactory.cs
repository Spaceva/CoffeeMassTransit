using System.Data;
using System.Data.SqlClient;

namespace WebinarMassTransit.Core.DAL
{
    public abstract class SqlConnectionFactory
    {
        public SqlConnectionFactory(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private string ConnectionString { get; }

        public IDbConnection Create()
        {
            return new SqlConnection(this.ConnectionString);
        }
    }
}