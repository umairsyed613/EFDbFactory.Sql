using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFDbFactory.Sql
{
    public interface IFactoryCreator
    {
        Task<IDbFactoryConnection> CreateFactoryWithTransaction(System.Data.IsolationLevel isolationLevel);
        Task<IDbFactoryConnection> CreateFactoryWithNoTransaction();
    }
}
