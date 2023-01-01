using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static void MigrateDatabase(this IServiceProvider serviceProvider, int? retry = 0)
        {
            int retryForAvailibility = retry!.Value;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Migration Postgresql started.");

                using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                            ProductName VARCHAR(24) NOT NULL,
                                                            Description TEXT,
                                                            Amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();

                logger.LogInformation("Migration Postgresql finished.");
            }
            catch (NpgsqlException exception)
            {
                logger.LogError(exception, " An error occurred while migrating Postgresql database.");

                if(retryForAvailibility < 10)
                {
                    retryForAvailibility++;
                    Thread.Sleep(3000);
                    serviceProvider.MigrateDatabase();
                }
            }
        }
    }
}
