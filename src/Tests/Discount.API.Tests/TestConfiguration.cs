using Microsoft.Extensions.Configuration;
using Shared.Tests;

namespace Discount.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(GlobalConstants.BasePath + "src\\Services\\Discount\\Discount.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}