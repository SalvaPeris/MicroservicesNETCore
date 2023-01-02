using Basket.API.Entities;
using Basket.API.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Basket.API.Tests
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private IDistributedCache? _redisCache;
        private IBasketRepository? _repository;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();
            var _configuration = TestConfiguration.GetConfiguration();
            services.AddStackExchangeRedisCache(options =>
             {
                 options.Configuration = _configuration.GetValue<string>("CacheSettings:ConnectionString");
             });

            var provider = services.BuildServiceProvider();

            _redisCache = provider.GetService<IDistributedCache>();
            _repository = new BasketRepository(_redisCache!);
        }

        [TestMethod]
        public async Task GetBasketAsync_Success()
        {
            //Act 
            var basket = await _repository!.GetBasket("testuser");

            //Assert 
            Assert.IsNull(basket);
        }

        [TestMethod]
        public async Task UpdateBasketAsync_Success()
        {
            //Act 
            ShoppingCart newBasket = new()
            {
                UserName = "testuser",
                Items =
                {
                    new ShoppingCartItem()
                    {
                        Quantity = 2,
                        Price = 34,
                        Color = "Red",
                        Description = "Description mobile",
                        ProductId = "602d2149e773f2a3990b47f5",
                        ProductName = "iPhone X"
                    },
                    new ShoppingCartItem()
                    {
                        Quantity = 1,
                        Price = 340,
                        Color = "Red",
                        Description = "Description mobile 2",
                        ProductId = "602d2149e773f2a3990b47f6",
                        ProductName = "iPhone XV"
                    }
                }
            };

            var basket = await _repository!.UpdateBasket(newBasket);

            //Assert 
            Assert.AreEqual(2, basket.Items.Count);
            Assert.AreEqual("testuser", basket.UserName);
            Assert.AreEqual(408, basket.TotalPrice);
        }

        [TestMethod]
        public async Task UpdateBasketWithDiscountGrpcAsync_Success()
        {
            //Act 
            ShoppingCart newBasket = new()
            {
                UserName = "testuser",
                Items =
                {
                    new ShoppingCartItem()
                    {
                        Quantity = 1,
                        Price = 1500,
                        Color = "Red",
                        Description = "Description mobile with discount of 150",
                        ProductId = "602d2149e773f2a3990b47f6",
                        ProductName = "IPhone X"
                    }
                }
            };

            var basket = await _repository!.UpdateBasket(newBasket);

            //Assert 
            Assert.AreEqual(1, basket.Items.Count);
            Assert.AreEqual("testuser", basket.UserName);
            Assert.AreEqual(1350, basket.TotalPrice);
        }
        [TestMethod]
        public async Task DeleteBasketAsync_Success()
        {
            //Act 
            await _repository!.DeleteBasket("testuser");

            var basket = await _repository!.GetBasket("testuser");

            //Assert 
            Assert.IsNull(basket);
        }

    }
}
