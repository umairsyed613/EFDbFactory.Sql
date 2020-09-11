using System;
using System.Data;
using System.Threading.Tasks;

using EFDbFactory.Sql.Options;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EFDbFactory.Sql
{
    public class DbFactory : IDbFactory
    {
        private readonly string _connectionString;
        private readonly ILoggerFactory _loggerFactory;
        private readonly bool _enableSensitiveDataLogging;
        private static bool _noCommitFactory;
        private static bool _readOnly;

        public SqlConnection Connection { get; private set; }

        public SqlTransaction Transaction { get; private set; }

        public DbFactory(string connectionString) => _connectionString = connectionString;

        public DbFactory(string connectionString, ILoggerFactory loggerFactory, bool enableSensitiveDataLogging)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException("ConnectionString cannot be empty!");
            _loggerFactory = loggerFactory;
            _enableSensitiveDataLogging = enableSensitiveDataLogging;
        }

        public DbFactory(EfDbFactoryOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new ArgumentNullException("ConnectionString cannot be empty!");
            }

            _connectionString = options.ConnectionString;
            _loggerFactory = options.LoggerFactory ?? null;
            _enableSensitiveDataLogging = options.EnableSensitiveDataLogging;
        }

        /// <summary>
        /// create factory with desired transaction isolation level.
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IDbFactory> Create(IsolationLevel isolationLevel) => await CreateReadWriteWithTransactionLevel(isolationLevel);

        /// <summary>
        /// create factory with no transaction
        /// </summary>
        /// <returns></returns>
        public async Task<IDbFactory> Create() => await CreateReadOnly();

        /// <summary>
        /// Create factory with 
        /// </summary>
        /// <returns></returns>
        public async Task<IDbFactory> CreateNoCommit()
        {
            _noCommitFactory = true;

            return await CreateReadWriteWithTransactionLevel(IsolationLevel.Unspecified);
        }

        public T For<T>()
        where T : EFDbContext
            => _loggerFactory != null ?
                   (_readOnly ? new ContextCreator<T>(Connection, Transaction, _loggerFactory, _enableSensitiveDataLogging).GetReadOnlyWithNoTracking() :
                        new ContextCreator<T>(Connection, Transaction, _loggerFactory, _enableSensitiveDataLogging).GetReadWriteWithDbTransaction()) :
                   (_readOnly ? new ContextCreator<T>(Connection, Transaction).GetReadOnlyWithNoTracking() :
                        new ContextCreator<T>(Connection, Transaction).GetReadWriteWithDbTransaction());

        /// <summary>
        /// Commit the transaction. throw exception when transaction is null. will not commit the transaction if the factory is created CreateNoCommit
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void CommitTransaction()
        {
            if (_noCommitFactory) { throw new InvalidOperationException("Cannot commit no commit factory."); }

            if (Transaction == null) { throw new InvalidOperationException("Cannot commit null transaction"); }

            Transaction.Commit();
        }

        private async Task<IDbFactory> CreateReadWriteWithTransactionLevel(IsolationLevel isolationLevel)
        {
            Connection = new SqlConnection(_connectionString);
            await Connection.OpenAsync();
            Transaction = Connection.BeginTransaction(isolationLevel);
            _readOnly = false;

            return this;
        }

        private async Task<IDbFactory> CreateReadOnly()
        {
            Connection = new SqlConnection(_connectionString);
            await Connection.OpenAsync();
            Transaction = null;
            _readOnly = true;

            return this;
        }

        public void Dispose()
        {
            if (_noCommitFactory) { Transaction?.Rollback(); }

            Connection?.Dispose();
            Transaction?.Dispose();
        }
    }
}
