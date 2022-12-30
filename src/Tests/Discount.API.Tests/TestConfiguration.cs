using Microsoft.Extensions.Configuration;

namespace Discount.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath("C:\\Users\\speri\\source\\repos\\MicroservicesNETCore\\src\\Services\\Discount\\Discount.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}