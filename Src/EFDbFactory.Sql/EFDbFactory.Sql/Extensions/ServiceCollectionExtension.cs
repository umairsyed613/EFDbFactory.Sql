using System;

using EFDbFactory.Sql.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EFDbFactory.Sql.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEfDbFactory(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            services.AddSingleton<IDbFactory, DbFactory>(options => new DbFactory(connectionString));

            return services;
        }

        public static IServiceCollection AddEfDbFactory(
            this IServiceCollection services,
            string connectionString,
            ILoggerFactory loggerFactory,
            bool enableSensitiveDataLogging = false)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            services.AddSingleton<IDbFactory, DbFactory>(options => new DbFactory(connectionString, loggerFactory, enableSensitiveDataLogging));

            return services;
        }

        public static IServiceCollection AddEfDbFactory(this IServiceCollection services, EfDbFactoryOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new ArgumentNullException("ConnectionString cannot be empty!");
            }

            services.AddSingleton<IDbFactory, DbFactory>(sp => new DbFactory(options));

            return services;
        }
    }
}
