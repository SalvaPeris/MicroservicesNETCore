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
                ProductName = "iPhone XV",
                Description = "Discount for iPhone XV",
                Amount = 500
            };
            await _repository!.CreateDiscount(coupon);

            Coupon result = await _repository.GetDiscount(coupon.ProductName);

            //Assert
            Assert.AreEqual(500, result.Amount);
        }

        [TestMethod]
        public async Task UpdateDiscountAsync_Success()
        {
            //Act
            string productName = "iPhone XV";
            Coupon coupon = await _repository!.GetDiscount(productName);
            //Update Amount from 10 to 20
            coupon.Amount = 20;
            await _repository!.UpdateDiscount(coupon);

            Coupon result = await _repository.GetDiscount(productName);

            //Assert
            Assert.AreEqual(20, result.Amount);
        }

        [TestMethod]
        public async Task RemoveDiscountAsync_Success()
        {
            string productName = "iPhone XV";

            //Act
            await _repository!.DeleteDiscount(productName);
            Coupon result = await _repository.GetDiscount(productName);

            //Assert
            Assert.AreEqual("No discount", result.ProductName);
        }
    }
}
