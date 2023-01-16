using Microsoft.Extensions.Configuration;
using Shared.Tests;

namespace Basket.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(GlobalConstants.BasePath + "src\\Services\\Basket\\Basket.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}
