using Basket.API.GrpcServices;
using Basket.API.Repository;
using Discount.Grpc.Protos;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region Redis Configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});
#endregion

#region General Configuration
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
#endregion

#region GRPC Configuration
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"])
    );
builder.Services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();
#endregion

#region MassTransit / RabbitMQ Configuration
builder.Services.AddMassTransit(configuration =>
{
    configuration.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["EventBusSettings:HostAddress"]);
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

app.Run();
