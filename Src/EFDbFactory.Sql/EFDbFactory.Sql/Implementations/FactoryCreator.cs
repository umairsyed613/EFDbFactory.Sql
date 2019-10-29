using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EFDbFactory.Sql
{
    public class FactoryCreator : IFactoryCreator
    {
        private readonly string _connectionString;

        public FactoryCreator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbFactoryConnection> CreateFactoryWithTransaction(IsolationLevel isolationLevel)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = connection.BeginTransaction(isolationLevel);
            return new DbFactoryConnection(connection, transaction);
        }

        public async Task<IDbFactoryConnection> CreateFactoryWithNoTransaction()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return new DbFactoryConnection(connection, null);
        }
    }
}
