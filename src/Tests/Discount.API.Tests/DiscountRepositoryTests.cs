using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Discount.API.Tests
{
    [TestClass]
    public class DiscountRepositoryTests
    {
        private IConfiguration? _configuration;
        private IDiscountRepository? _repository;

        [TestInitialize]
        public void Setup()
        {
            _configuration = TestConfiguration.GetConfiguration();
            _repository = new DiscountRepository(_configuration);
        }

        [TestMethod]
        public async Task CreateAndGetDiscountAsync_Success()
        {
            //Act
            Coupon coupon = new()
            {
                ProductName = "iPhone X",
                Description = "Discount for iPhone X",
                Amount = 10
            };
            await _repository!.CreateDiscount(coupon);

            Coupon result = await _repository.GetDiscount(coupon.ProductName);

            //Assert
            Assert.AreEqual(10, result.Amount);
        }
    }
}
