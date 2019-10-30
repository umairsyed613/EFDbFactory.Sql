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

        /// <summary>
        /// create factory with desired transaction isolation level.
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IDbFactoryConnection> Create(IsolationLevel isolationLevel)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var transaction = connection.BeginTransaction(isolationLevel);
            return new DbFactoryConnection(connection, transaction);
        }

        /// <summary>
        /// create factory with no transaction
        /// </summary>
        /// <returns></returns>
        public async Task<IDbFactoryConnection> Create()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return new DbFactoryConnection(connection, null);
        }
    }
}
