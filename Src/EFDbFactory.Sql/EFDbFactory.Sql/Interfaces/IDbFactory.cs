namespace EFDbFactory.Sql
{
    public interface IDbFactory<out T> where T : CommonDbContext
    {
        /// <summary>
        /// return readwrite context
        /// </summary>
        /// <returns></returns>
        T GetReadWriteWithDbTransaction();

        /// <summary>
        /// return readonly context with no tracking of your EF entity object
        /// </summary>
        /// <returns></returns>
        T GetReadOnlyWithNoTracking();
    }
}
