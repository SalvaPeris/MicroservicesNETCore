using Microsoft.Extensions.Configuration;
namespace Ordering.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath("C:\\Users\\speri\\source\\repos\\MicroservicesNETCore\\src\\Services\\Ordering\\Ordering.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}
