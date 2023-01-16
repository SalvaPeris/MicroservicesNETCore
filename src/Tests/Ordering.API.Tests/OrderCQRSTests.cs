using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.API.Tests
{
    [TestClass]
    public class OrderCQRSTests
    {
        private IMapper? _mapper;
        private OrderContext? _context;
        private IOrderRepository? _repository;
        string _userName = "Test";
        string _newUserName = "TestUpdated";

        [TestInitialize]
        public void Setup()
        {
            var _configuration = TestConfiguration.GetConfiguration();

            var services = new ServiceCollection();
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("OrderingConnectionString"));
            });

            var provider = services.BuildServiceProvider();
            _context = provider.GetService<OrderContext>();

            _repository = new OrderRepository(_context!);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderVM>().ReverseMap();
                cfg.CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
                cfg.CreateMap<Order, UpdateOrderCommand>().ReverseMap();
            }
                
            );
            _mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task GetOrderAsync_Success()
        {
            var query = new GetOrdersListQuery(_userName);
            var handler = new GetOrdersListQueryHandler(_repository, _mapper);
            var response = await handler.Handle(query, new CancellationToken());

            Assert.AreEqual(1, response.Count);
        }

        [TestMethod]
        public async Task AddOrderAsync_Success()
        {
            var command = new CheckoutOrderCommand()
            {
                UserName = _userName
            };

            var logger = Mock.Of<ILogger<CheckoutOrderCommandHandler>>();
            var emailService = Mock.Of<IEmailService>();

            var handler = new CheckoutOrderCommandHandler(_repository!, _mapper!, emailService, logger);
            var response = await handler.Handle(command, new CancellationToken());

            var result = await GetOrderAsync(_userName);

            Assert.AreEqual(true, response > 0);
        }

        [TestMethod]
        public async Task UpdateOrderAsync_Success()
        {
            var result = await GetOrderAsync(_userName);
            var command = new UpdateOrderCommand()
            {
                Id = result.FirstOrDefault()!.Id,
                UserName = _newUserName
            };

            var logger = Mock.Of<ILogger<UpdateOrderCommandHandler>>();

            var handler = new UpdateOrderCommandHandler(_repository!, _mapper!, logger);
            var response = await handler.Handle(command, new CancellationToken());

            var resultFinal = await GetOrderAsync(_newUserName);
            Assert.AreEqual(1, resultFinal.Count);
        }

        [TestMethod]
        public async Task DeleteOrderAsync_Success()
        {
            var result = await GetOrderAsync(_newUserName);

            var command = new DeleteOrderCommand()
            {
                Id = result.FirstOrDefault()!.Id
            };

            var logger = Mock.Of<ILogger<DeleteOrderCommandHandler>>();

            var handler = new DeleteOrderCommandHandler(_repository!, _mapper!, logger);
            await handler.Handle(command, new CancellationToken());

            var response = await GetOrderAsync(_newUserName);

            Assert.AreEqual(0, response.Count);
        }

        private async Task<List<OrderVM>> GetOrderAsync(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var handler = new GetOrdersListQueryHandler(_repository, _mapper);
            return await handler.Handle(query, new CancellationToken());
        }

    }
}

