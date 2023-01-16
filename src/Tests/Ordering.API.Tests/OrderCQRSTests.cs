using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ordering.Application;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ordering.API.Tests
{
    [TestClass]
    public class OrderCQRSTests
    {
        private IMapper? _mapper;
        private OrderContext? _context;
        private IOrderRepository? _repository;
        private IMediator? _mediator;
        string _userName = "Test";
        string _newUserName = "TestUpdated";

        [TestInitialize]
        public void Setup()
        {
            var _configuration = TestConfiguration.GetConfiguration();

            var services = new ServiceCollection();

            services.AddApplicationServices();
            services.AddInfrastructureServices(_configuration);

            var provider = services.BuildServiceProvider();

            _context = provider.GetService<OrderContext>();
            _mediator = provider.GetService<IMediator>();

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
            var response = await _mediator!.Send(query);
            Assert.AreEqual(1, response.Count);
        }

        [TestMethod]
        public async Task AddOrderAsync_Success()
        {
            var command = new CheckoutOrderCommand()
            {
                UserName = _userName,
                EmailAddress = "sperisgimeno@gmail.com",
                TotalPrice = 500
            };

            var response = await _mediator!.Send(command);

            Assert.AreEqual(true, response > 0);
        }

        [TestMethod]
        public async Task UpdateOrderAsync_Success()
        {
            var result = await GetOrderAsync(_userName);
            var command = new UpdateOrderCommand()
            {
                Id = result.FirstOrDefault()!.Id,
                UserName = _newUserName,
                EmailAddress = result.FirstOrDefault()!.EmailAddress,
                TotalPrice = result.FirstOrDefault()!.TotalPrice
            };

            var response = await _mediator!.Send(command);
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

            await _mediator!.Send(command);
            var response = await GetOrderAsync(_newUserName);

            Assert.AreEqual(0, response.Count);
        }

        private async Task<List<OrderVM>> GetOrderAsync(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var response = await _mediator!.Send(query);
            return response;
        }

    }
}

