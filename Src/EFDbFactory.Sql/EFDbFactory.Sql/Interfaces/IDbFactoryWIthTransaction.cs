using System;
using Microsoft.Data.SqlClient;

namespace UN.DbFactory.Sql.Interfaces
{
    public interface IDbFactoryWIthTransaction : IDisposable
    {
        IDbFactory<T> FactoryFor<T>() where T : CommonDbContext;
        void CommitTransaction();
        SqlConnection Connection { get; }
        SqlTransaction Transaction { get; }
    }
}
