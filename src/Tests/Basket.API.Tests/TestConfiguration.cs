using Microsoft.Extensions.Configuration;

namespace Basket.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath("C:\\Users\\speri\\source\\repos\\MicroservicesNETCore\\src\\Services\\Basket\\Basket.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }

        public static IConfiguration GetDiscountConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath("C:\\Users\\speri\\source\\repos\\MicroservicesNETCore\\src\\Services\\Discount\\Discount.Grpc")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}
