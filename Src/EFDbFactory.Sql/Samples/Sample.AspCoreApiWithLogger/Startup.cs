using EFDbFactory.Sql.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.AspCoreApiWithLogger.Services;

namespace Sample.AspCoreApiWithLogger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => builder
                                             .AddFilter((category, level) =>
                                                            category == DbLoggerCategory.Database.Command.Name
                                                         && level == LogLevel.Information)
                                             .AddConsole());

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEfDbFactory(Configuration.GetConnectionString("DbConnection"), MyLoggerFactory, true);
            services.AddSingleton<IQuizService, QuizService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
