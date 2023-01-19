
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

internal class Program
{
	async static Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Host.ConfigureLogging((hostingContext, loggingBuilder) =>
			{
				loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
				loggingBuilder.AddDebug();
			})
			.ConfigureServices(services =>
			{
				services.AddOcelot();
			});

		var app = builder.Build();

		await app.UseOcelot();

		app.Run();
	}
}