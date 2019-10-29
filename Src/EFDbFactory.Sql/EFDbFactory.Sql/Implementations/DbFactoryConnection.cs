using System;
using Microsoft.Data.SqlClient;

namespace EFDbFactory.Sql
{
    public class DbFactoryConnection : IDbFactoryConnection
    {
        public DbFactoryConnection(SqlConnection connection, SqlTransaction transaction)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        public IDbFactory<T> FactoryFor<T>() where T : CommonDbContext => new DbFactory<T>(Connection, Transaction);

        public void CommitTransaction()
        {
            Transaction.Commit();
        }

        public SqlConnection Connection { get; }
        public SqlTransaction Transaction { get; }

        public void Dispose()
        {
            Transaction.Dispose();
            Connection.Dispose();
        }
    }
}
