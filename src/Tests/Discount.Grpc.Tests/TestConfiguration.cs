using Microsoft.Extensions.Configuration;

namespace Discount.Grpc.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath("C:\\Users\\speri\\source\\repos\\MicroservicesNETCore\\src\\Services\\Discount\\Discount.Grpc")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}