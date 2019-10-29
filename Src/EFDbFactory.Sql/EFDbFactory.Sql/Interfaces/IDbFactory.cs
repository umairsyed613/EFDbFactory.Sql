namespace EFDbFactory.Sql
{
    public interface IDbFactory<out T> where T : CommonDbContext
    {
        T GetReadWriteWithDbTransaction();
        T GetReadOnlyWithNoTracking();
    }
}
