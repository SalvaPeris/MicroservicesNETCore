
using Basket.API.GrpcServices;
using Basket.API.Repository;
using Discount.Grpc.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"])
    );
builder.Services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();

// MassTransit / RabbitMQ Configuration
builder.Services.AddMassTransit(configuration =>
{
    configuration.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

//New version no longer requieres the AddMassTransitHostedService -> https://stackoverflow.com/questions/72403579/workerservice-configure-a-rabbitmq-with-masstransit
//builder.Services.AddMassTransitHostedService();

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

app.Run();
