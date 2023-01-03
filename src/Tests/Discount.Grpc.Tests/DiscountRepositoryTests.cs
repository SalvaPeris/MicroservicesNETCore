using Discount.Grpc.Repositories;
using Discount.Grpc.Services;
using Grpc.Core.Testing;
using Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Grpc.Core.Utils;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Tests
{
    [TestClass]
    public class DiscountRepositoryTests
    {
        private IMapper? _mapper;
        private DiscountService? _discountService;
        private ServerCallContext? _serverCallContext;

        [TestInitialize]
        public void Setup()
        {
            var _configuration = TestConfiguration.GetConfiguration();

            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Coupon, CouponModel>().ReverseMap()
            );
            _mapper = config.CreateMapper();

            var discountRepository = new DiscountRepository(_configuration);
            var logger = Mock.Of<ILogger<DiscountService>>();
            _discountService = new DiscountService(discountRepository, _mapper!, logger);
            _serverCallContext = CreateTestContext(_discountService);
            
        }

        public static ServerCallContext CreateTestContext(DiscountService discountService)
        {
            return TestServerCallContext.Create(nameof(discountService.GetDiscount),
                null,
               DateTime.UtcNow.AddHours(1),
                new Metadata(),
               CancellationToken.None,
               "127.0.0.1",
                null,
                null,
               (metadata) => TaskUtils.CompletedTask,
               () => new WriteOptions(),
               (writeOptions) => { });
        }

        [TestMethod]
        public async Task CreateAndGetDiscountAsync_Success()
        {
            var request = new GetDiscountRequest
            {
                ProductName = "IPhone X"
            };
            var response = await _discountService!.GetDiscount(request, _serverCallContext!);
            Assert.AreEqual(150, response.Amount);
        }
    }
}