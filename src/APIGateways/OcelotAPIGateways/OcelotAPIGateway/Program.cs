using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

internal class Program
{
	async static Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Host
			.ConfigureAppConfiguration((hostingContext, configuration) =>
			{
				configuration.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
			})
			.ConfigureLogging((hostingContext, loggingBuilder) =>
			{
				loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
				loggingBuilder.AddDebug();
			})
			.ConfigureServices(services =>
			{
				services.AddOcelot()
						.AddCacheManager(settings => settings.WithDictionaryHandle());
			});

		var app = builder.Build();

		await app.UseOcelot();

		app.Run();
	}
}