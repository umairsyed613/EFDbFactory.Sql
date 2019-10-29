using System;
using System.Collections.Generic;
using System.Text;

namespace UN.DbFactory.Sql.Interfaces
{
    public interface IDbFactory<out T> where T : CommonDbContext
    {
        T GetReadWriteWithDbTransaction();
        T GetReadOnlyWithNoTracking();
    }
}
