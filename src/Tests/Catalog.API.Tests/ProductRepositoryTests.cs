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

        private IConfiguration? _configuration;
        private CatalogContext? _context;
        private IProductRepository? _repository;

        [TestInitialize]
        public void Setup()
        {
            _configuration = TestConfiguration.GetConfiguration();
            _context = new CatalogContext(_configuration);
            _repository = new ProductRepository(_context!);
        }

        [TestMethod]
        public async Task GetProductsAsync_Success()
        {
            //Act 
            IEnumerable<Product> result = await _repository!.GetProducts();

            //Assert 
            Assert.AreEqual(6, result.Count());
        }

        [TestMethod]
        public async Task GetProductById_Success()
        {
            //Act 
            string productId = "602d2149e773f2a3990b47f5";
            var result = await _repository!.GetProduct(productId);
            string productName = "IPhone X";

            //Assert 
            Assert.AreEqual(productName, result?.Name);
        }

        [TestMethod]
        public async Task GetProductByCategory_Success()
        {
            //Act 
            string productCategory = "Smart Phone";
            var result = await _repository!.GetProductsByCategory(productCategory);

            //Assert 
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public async Task CreateProduct_Success()
        {
            //Act 
            Product newProduct = new()
            {
                Name = "Test Mobile",
                Description = "Test Description",
                Summary = "Test Summary",
                Category = "Smart Phone",
                Price = 34
            };

            await _repository!.CreateProduct(newProduct);
            IEnumerable<Product> resultCount = await _repository.GetProducts();

            //Assert 
            Assert.AreEqual(7, resultCount.Count());
        }

        [TestMethod]
        public async Task UpdateProduct_Success()
        {
            //Act 
            string productId = "602d2149e773f2a3990b47f5";
            var product = await _repository!.GetProduct(productId);
            string? oldName;

            if (product != null)
            {
                oldName = product.Name;
                string newName = "IPhone XI";
                product.Name = newName;

                var result = await _repository.UpdateProduct(product);
                product = await _repository.GetProduct(productId);

                //Restore product to old name
                var product2 = new Product()
                {
                    Id = product.Id,
                    Name = "IPhone X",
                    Summary = product.Summary,
                    Description = product.Description,
                    ImageFile = product.ImageFile,
                    Category = product.Category,
                    Price = product.Price
                };

                await _repository.UpdateProduct(product2);

                //Assert
                if (result)
                {
                    Assert.AreEqual(newName, product.Name);
                }
                else
                {
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
            IEnumerable<Product> products = await _repository!.GetProducts();
            await _repository.DeleteProduct(products?.LastOrDefault()!.Id!);
            IEnumerable<Product> resultCount = await _repository.GetProducts();

            //Assert 
            Assert.AreEqual(6, resultCount.Count());
        }
    }
}