using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

#region MassTransit / RabbitMQ Configuration
builder.Services.AddMassTransit(configuration =>
{
    configuration.AddConsumer<BasketCheckoutConsumer>();

    configuration.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        configurator.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

//New version no longer requieres the AddMassTransitHostedService method -> https://stackoverflow.com/questions/72403579/workerservice-configure-a-rabbitmq-with-masstransit
//builder.Services.AddMassTransitHostedService();

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<OrderContext>((context, service) =>
{
    //These lines are an action parameter, in MigrationDatabase > seeder will execute SeedAsync.
    var logger = app.Services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger!).Wait();
});

app.Run();
