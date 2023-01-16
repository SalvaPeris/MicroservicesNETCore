using Microsoft.Extensions.Configuration;
using Shared.Tests;

namespace Ordering.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(GlobalConstants.BasePath + "src\\Services\\Ordering\\Ordering.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}
