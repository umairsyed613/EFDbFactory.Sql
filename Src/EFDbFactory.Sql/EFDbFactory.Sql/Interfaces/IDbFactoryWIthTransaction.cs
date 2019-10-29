using System;
using Microsoft.Data.SqlClient;

namespace EFDbFactory.Sql
{
    public interface IDbFactoryWIthTransaction : IDisposable
    {
        IDbFactory<T> FactoryFor<T>() where T : CommonDbContext;
        void CommitTransaction();
        SqlConnection Connection { get; }
        SqlTransaction Transaction { get; }
    }
}
