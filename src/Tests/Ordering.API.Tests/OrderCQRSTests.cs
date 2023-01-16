using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.API.Tests
{
    public class OrderCQRSTests
    {
        private IMapper? _mapper;
        private OrderContext? _context;
        private IOrderRepository? _repository;
        private IConfiguration? _configuration;

        [TestInitialize]
        public void Setup()
        {
            _context = new OrderContext();
            _repository = new OrderRepository(_context!);

            _configuration = TestConfiguration.GetConfiguration();

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
            var query = new GetOrdersListQuery("spg");
            var handler = new GetOrdersListQueryHandler(_repository, _mapper);
            var response = await handler.Handle(query, new CancellationToken());

            Assert.AreEqual(1, response.Count);
        }

    }
}

