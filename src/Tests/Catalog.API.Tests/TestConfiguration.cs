using Microsoft.Extensions.Configuration;
using Shared.Tests;

namespace Catalog.API.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(GlobalConstants.BasePath + "src\\Services\\Catalog\\Catalog.API")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}
