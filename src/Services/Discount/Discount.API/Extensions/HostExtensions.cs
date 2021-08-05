using System.Threading;
using Dapper;
using Discount.API.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int retry = 0)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<IDiscountContext>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var connection = context.Connection;
                try
                {
                    logger.LogInformation("Start migrating ...");
                    connection.Execute("DROP TABLE IF EXISTS Coupon");
                    connection.Execute("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
                    connection.Execute(@"CREATE TABLE Coupon(
	                                        Id uuid DEFAULT uuid_generate_v4(),
	                                        ProductName VARCHAR(24) NOT NULL,
	                                        Description TEXT,
                                            Amount INT NOT NULL,
	                                        PRIMARY KEY (id));");
                    connection.Execute("INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 21', 'Samsung', 10)");
                    connection.Execute("INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone 12', 'Apple', 10)");
                    logger.LogInformation("Migrated successfully!");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating database");
                    if (retry < 3)
                    {
                        retry++;
                        Thread.Sleep(1000);
                        MigrateDatabase<TContext>(host, retry);
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
    }
}
