using Microsoft.Extensions.Configuration;

namespace Catalog.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath("C:\\Users\\speri\\source\\repos\\MicroservicesNETCore\\src\\Services\\Catalog\\Catalog.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}
