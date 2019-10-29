using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using UN.DbFactory.Sql.Interfaces;

namespace UN.DbFactory.Sql.Implementations
{
    public class DbFactory<T> : IDbFactory<T> where T : CommonDbContext
    {
        private readonly string _connectionString;
        private readonly DbConnection _outerConnection;
        private readonly DbTransaction _transaction;

        public DbFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbFactory(DbConnection connection, DbTransaction transaction)
        {
            _outerConnection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = transaction;
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

            options = _connectionString != null ? options.UseSqlServer(_connectionString) : options.UseSqlServer(_outerConnection);

            return (T) Activator.CreateInstance(typeof(T), options.Options);
        }
    }
}
