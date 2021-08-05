using System;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(
            this IHost host, 
            Action<TContext, IServiceProvider> seeder,
            int retry = 0) 
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");
                    InvokeSeeder(seeder, context, services);
                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");

                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating database");
                    if (retry < 3)
                    {
                        retry++;
                        Thread.Sleep(1000);
                        MigrateDatabase(host, seeder, retry);
                    }
                    else
                    {
                        logger.LogError(ex, "Failed to migrate database");
                        throw;
                    }
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(
            Action<TContext, IServiceProvider> seeder,
            TContext context,
            IServiceProvider services) 
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
