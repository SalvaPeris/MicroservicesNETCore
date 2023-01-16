using Microsoft.Extensions.Configuration;
using Shared.Tests;

namespace Discount.Grpc.Tests
{
    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(GlobalConstants.BasePath + "src\\Services\\Discount\\Discount.Grpc")
              .AddJsonFile("appsettings.json")
              .Build();
            return config;
        }
    }
}