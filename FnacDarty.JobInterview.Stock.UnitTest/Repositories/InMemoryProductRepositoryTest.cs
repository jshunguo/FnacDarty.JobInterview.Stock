using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.UnitTest.Repositories
{
    [TestFixture]
    public class InMemoryProductRepositoryTest
    {
        private InMemoryProductRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new InMemoryProductRepository();
        }

        [Test]
        public void AddProduct_ValidProduct_ProductAdded()
        {
            var product = new Product("EAN123");

            _repository.AddProduct(product);

            Assert.AreEqual(product, _repository.FindProductById("EAN123"));
        }

        [Test]
        public void AddProducts_MultipleValidProducts_ProductsAdded()
        {
            var products = new List<Product>
        {
            new Product("EAN123"),
            new Product("EAN456")
        };

            _repository.AddProducts(products);

            Assert.AreEqual(products[0], _repository.FindProductById("EAN123"));
            Assert.AreEqual(products[1], _repository.FindProductById("EAN456"));
        }

        [Test]
        public void FindProductById_ProductExists_ReturnsProduct()
        {
            var product = new Product("EAN123");
            _repository.AddProduct(product);

            var foundProduct = _repository.FindProductById("EAN123");

            Assert.AreEqual(product, foundProduct);
        }

        [Test]
        public void FindProductById_ProductDoesNotExist_ReturnsDefault()
        {
            var foundProduct = _repository.FindProductById("EAN123");
            var expectedProduct = default(Product);

            Assert.AreEqual(expectedProduct, foundProduct);
        }

        [Test]
        public void FilterExistingProductIds_ExistingIds_ReturnsExistingIds()
        {
            var product1 = new Product("EAN123");
            var product2 = new Product("EAN456");
            _repository.AddProduct(product1);
            _repository.AddProduct(product2);

            var ids = _repository.FilterExistingProductIds(new[] { "EAN123", "EAN456", "EAN789" });

            Assert.IsTrue(ids.SequenceEqual(new[] { "EAN123", "EAN456" }));
        }

        [Test]
        public void IsProductExisting_ProductExists_ReturnsTrue()
        {
            var product = new Product("EAN123");
            _repository.AddProduct(product);

            Assert.IsTrue(_repository.IsProductExisting("EAN123"));
        }

        [Test]
        public void IsProductExisting_ProductDoesNotExist_ReturnsFalse()
        {
            Assert.IsFalse(_repository.IsProductExisting("EAN123"));
        }

        [Test]
        public void AddProduct_NullProduct_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.AddProduct(default));
        }

        [Test]
        public void FindProductById_NullOrWhiteSpaceProductId_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.FindProductById(null));
            Assert.Throws<ArgumentNullException>(() => _repository.FindProductById(""));
            Assert.Throws<ArgumentNullException>(() => _repository.FindProductById(" "));
        }

        [Test]
        public void FilterExistingProductIds_NullProductIds_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.FilterExistingProductIds(null));
        }
    }
}
