using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using TaskManagement.Infrastructure.Persistence.DataLayer;

namespace TaskManagement.Infrastructure.Persistence.ExtensionMethods
{
    public static class ServiceExtensions
    {
        public static void AddLoggerLayer(this IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "LogEvents",
                SchemaName = "dbo",
                AutoCreateSqlTable = true,
                BatchPostingLimit = 1000,
                BatchPeriod = new TimeSpan(0, 0, 10)
            };

            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .WriteTo.MSSqlServer(
                        connectionString: connectionString,
                        sinkOptions: sinkOptions)
                    .CreateLogger();
        }
    }
}
