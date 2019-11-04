using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EFDbFactory.Sql
{
    public class FactoryCreator : IFactoryCreator
    {
        private readonly string _connectionString;
        private readonly ILoggerFactory _loggerFactory;
        private readonly bool _enableSensitiveDataLogging;

        public FactoryCreator(string connectionString) => _connectionString = connectionString;

        public FactoryCreator(string connectionString, ILoggerFactory loggerFactory, bool enableSensitiveDataLogging)
        {
            _connectionString = connectionString;
            _loggerFactory = loggerFactory;
            _enableSensitiveDataLogging = enableSensitiveDataLogging;
        }

        /// <summary>
        /// create factory with desired transaction isolation level.
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IFactoryCreator> Create(IsolationLevel isolationLevel) => await CreateReadWrite(isolationLevel);

        /// <summary>
        /// create factory with no transaction
        /// </summary>
        /// <returns></returns>
        public async Task<IFactoryCreator> Create() => await CreateReadOnly();

        public IDbFactory<T> FactoryFor<T>() where T : CommonDbContext => _loggerFactory != null ? new DbFactory<T>(Connection, Transaction, _loggerFactory, _enableSensitiveDataLogging) : new DbFactory<T>(Connection, Transaction);

        public void CommitTransaction()
        {
            if (Transaction == null) { throw new InvalidOperationException("Cannot commit null transaction"); }

            Transaction.Commit();
        }

        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; private set; }

        private async Task<IFactoryCreator> CreateReadWrite(IsolationLevel isolationLevel)
        {
            Connection = new SqlConnection(_connectionString);
            await Connection.OpenAsync();
            Transaction = Connection.BeginTransaction(isolationLevel);
            return this;
        }

        private async Task<IFactoryCreator> CreateReadOnly()
        {
            Connection = new SqlConnection(_connectionString);
            await Connection.OpenAsync();
            Transaction = null;
            return this;
        }

        public void Dispose()
        {
            Connection?.Dispose();
            Transaction?.Dispose();
        }
    }
}
