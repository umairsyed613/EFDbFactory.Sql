using System.Threading.Tasks;

namespace EFDbFactory.Sql
{
    public interface IFactoryCreator
    {
        Task<IDbFactoryConnection> Create(System.Data.IsolationLevel isolationLevel);
        Task<IDbFactoryConnection> Create();
    }
}
