using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFDbFactory.Sql
{
    public class DbFactory<T> : IDbFactory<T> where T : CommonDbContext
    {
        private readonly string _connectionString;
        private readonly DbConnection _outerConnection;
        private readonly DbTransaction _transaction;
        private readonly ILoggerFactory _loggerFactory;
        private readonly bool _enableSensitiveDataLogging;

        public DbFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbFactory(string connectionString, ILoggerFactory loggerFactory)
        {
            _connectionString = connectionString;
            _loggerFactory = loggerFactory;
        }

        public DbFactory(DbConnection connection, DbTransaction transaction)
        {
            _outerConnection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
        }

        public DbFactory(DbConnection connection, DbTransaction transaction, ILoggerFactory loggerFactory, bool enableSensitiveDataLogging)
        {
            _outerConnection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
            _loggerFactory = loggerFactory;
            _enableSensitiveDataLogging = enableSensitiveDataLogging;
        }

        public T GetReadOnlyWithNoTracking()
        {
            var context = CreateDbContext();
            context.ReadOnlyMode = true;
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.LazyLoadingEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context;
        }

        public T GetReadWriteWithDbTransaction()
        {
            var context = CreateDbContext();
            if (_transaction != null) { context.Database.UseTransaction(_transaction); }

            context.ChangeTracker.AutoDetectChangesEnabled = true;

            return context;
        }

        private T CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<T>();

            if (_loggerFactory != null)
            {
                options.UseLoggerFactory(_loggerFactory);

                options.EnableSensitiveDataLogging(_enableSensitiveDataLogging);
            }

            options = _connectionString != null ? options.UseSqlServer(_connectionString) : options.UseSqlServer(_outerConnection);

            return (T) Activator.CreateInstance(typeof(T), options.Options);
        }
    }
}
