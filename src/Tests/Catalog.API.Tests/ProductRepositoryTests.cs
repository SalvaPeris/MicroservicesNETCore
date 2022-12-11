using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Catalog.API.Tests
{
    [TestClass]
    public class ProductRepositoryTests
    {

        private IConfiguration _configuration;

        [TestInitialize]
        public void Setup()
        {
            _configuration = TestConfiguration.getConfiguration();
        }

        [TestMethod]
        public void CatalogContext_Constructor_Success()
        {

            //Act 
            var context = new CatalogContext(_configuration);

            //Assert 
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public async Task GetProductsAsync_Success()
        {
            //Act 
            var context = new CatalogContext(_configuration);

            IProductRepository _repository = new ProductRepository(context);

            IEnumerable<Product> result = await _repository.GetProducts();

            //Assert 
            Assert.AreEqual(6, result.Count());
        }

        [TestMethod]
        public async Task GetProductByName_Success()
        {
            //Act 
            var context = new CatalogContext(_configuration);

            IProductRepository _repository = new ProductRepository(context);

            var result = await _repository.GetProductByName("IPhone X");

            string productId = "602d2149e773f2a3990b47f5";

            //Assert 
            Assert.AreEqual(productId, result?.FirstOrDefault()?.Id);
        }

        [TestMethod]
        public async Task GetProductById_Success()
        {
            //Act 
            var context = new CatalogContext(_configuration);

            IProductRepository _repository = new ProductRepository(context);

            string productId = "602d2149e773f2a3990b47f5";

            var result = await _repository.GetProduct(productId);

            string productName = "IPhone X";

            //Assert 
            Assert.AreEqual(productName, result?.Name);
        }

        [TestMethod]
        public async Task CreateProduct_Success()
        {
            //Act 
            var context = new CatalogContext(_configuration);

            IProductRepository _repository = new ProductRepository(context);

            Product newProduct = new()
            {
                Name = "Test Mobile",
                Description = "Test Description",
                Summary = "Test Summary",
                Category = "Smart Phone",
                Price = 34
            };

            await _repository.CreateProduct(newProduct);

            IEnumerable<Product> resultCount = await _repository.GetProducts();

            //Assert 
            Assert.AreEqual(7, resultCount.Count());
        }

        [TestMethod]
        public async Task UpdateProduct_Success()
        {
            //Act 
            var context = new CatalogContext(_configuration);

            IProductRepository _repository = new ProductRepository(context);

            string productId = "602d2149e773f2a3990b47f5";

            var product = await _repository.GetProduct(productId);

            string? oldName;

            if (product != null)
            {
                oldName = product.Name;
                string newName = "IPhone XI";
                product.Name = newName;

                var result = await _repository.UpdateProduct(product);

                //Assert
                if (result)
                {
                    product = await _repository.GetProduct(productId);
                    Assert.AreEqual(newName, product.Name);
                }
                else
                {
                    product = await _repository.GetProduct(productId);
                    Assert.AreEqual(oldName, product.Name);
                }
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task DeleteProduct_Success()
        {
            //Act 
            var context = new CatalogContext(_configuration);

            IProductRepository _repository = new ProductRepository(context);

            IEnumerable<Product> products = await _repository.GetProducts();

            var result = await _repository.DeleteProduct(products?.LastOrDefault().Id);

            IEnumerable<Product> resultCount = await _repository.GetProducts();

            //Assert 
            Assert.AreEqual(6, resultCount.Count());
        }
    }
}