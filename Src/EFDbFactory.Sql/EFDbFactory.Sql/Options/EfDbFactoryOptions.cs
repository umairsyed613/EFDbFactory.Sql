using Microsoft.Extensions.Logging;

namespace EFDbFactory.Sql.Options
{
    public class EfDbFactoryOptions
    {
        public string ConnectionString { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public bool EnableSensitiveDataLogging { get; set; }
    }
}
