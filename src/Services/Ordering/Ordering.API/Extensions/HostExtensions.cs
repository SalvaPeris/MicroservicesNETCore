using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static void MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext: DbContext
        {
            int retryForAvailability = retry!.Value;

            using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migration {DbContextName} started", typeof(TContext));
                InvokeSeeder(seeder!, context!, services);
                logger.LogInformation("Migration {DbContextName} finished", typeof(TContext));
            }
            catch (SqlException e)
            {
                logger.LogInformation("An error ocurred with database migration: {e}", e);

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                }
            }
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
