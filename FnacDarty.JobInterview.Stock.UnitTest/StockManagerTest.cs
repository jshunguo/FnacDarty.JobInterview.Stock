using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Factories;
using FnacDarty.JobInterview.Stock.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestFixture]
    internal class StockManagerTest
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private Mock<IProductFactory> _productFactoryMock;
        private Mock<IStockMovementFactory> _stockMovementFactory;

        [SetUp]
        public void SetUp()
        {
            _productFactoryMock = new Mock<IProductFactory>(MockBehavior.Strict);
            _stockMovementFactory = new Mock<IStockMovementFactory>(MockBehavior.Strict);
            _productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>(MockBehavior.Strict);
        }

        private void SetUpMocksForAdd(StockMovement lastInventory, StockMovement stockMovement)
        {
            var date = stockMovement.Date;
            var label = stockMovement.Label;
            var productId = stockMovement.Product.Id;
            var quantity = stockMovement.Quantity;

            _productFactoryMock.Setup(pf => pf.Get(productId)).Returns(new Product(productId));
            _productRepositoryMock.Setup(pr => pr.IsProductExisting(productId)).Returns(false);
            _productRepositoryMock.Setup(pr => pr.AddProduct(It.Is<Product>(p => p.Id == productId)));
            _stockMovementFactory.Setup(sf => sf.GetStock(lastInventory, date, label, productId, quantity)).Returns(stockMovement);
            _stockMovementRepositoryMock.Setup(sr => sr.GetLatestInventoryMovementForProduct(productId)).Returns(lastInventory);
            _stockMovementRepositoryMock.Setup(sr => sr.AddMovement(It.Is<StockMovement>(sm => sm.Equals(stockMovement))));
        }

        private IStockManager CreateStockManager()
        {
            return new StockManager(_productFactoryMock.Object, _stockMovementFactory.Object, 
                _productRepositoryMock.Object, _stockMovementRepositoryMock.Object);
        }

        private static StockMovement CreateSingleStockMovement(DateTime dateAfter, string label, string productId, long quantity)
        {
            return new StockMovement(dateAfter, label, new Product(productId), quantity);
        }

        private static bool AreSame(IEnumerable<Product> products, IEnumerable<string> productIds)
        {
            return products.Select(item => item.Id).SequenceEqual(productIds);
        }

        private static bool AreSame(IEnumerable<StockMovement> first, IEnumerable<StockMovement> second)
        {
            var expected = new HashSet<StockMovement>(first);
            var actual = new HashSet<StockMovement>(second);
            var except = actual.Except(expected).ToList();
            return except.Count == 0;
        }

        private static void AssertMovement(StockMovement expectedMovement, StockMovement actualMovement)
        {
            Assert.IsNotNull(actualMovement);
            Assert.AreEqual(expectedMovement.Date, actualMovement.Date);
            Assert.AreEqual(expectedMovement.Label, actualMovement.Label);
            Assert.AreEqual(expectedMovement.Product, actualMovement.Product);
            Assert.AreEqual(expectedMovement.Quantity, actualMovement.Quantity);
        }

        private static void AssertMovements(IEnumerable<StockMovement> expectedMovements, IEnumerable<StockMovement> actualMovements)
        {
            var expected = new HashSet<StockMovement>(expectedMovements);
            var actual = new HashSet<StockMovement>(actualMovements);
            var except = actual.Except(expected).ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(0, except.Count);
        }

        private static void AssertDictionaries(IDictionary<string, long> expectedDictionary, IDictionary<string, long> actualDictionary)
        {
            Assert.AreEqual(expectedDictionary.Count, actualDictionary.Count);

            foreach (var expectedKey in expectedDictionary.Keys)
            {
                Assert.IsTrue(actualDictionary.ContainsKey(expectedKey));

                var expectedValue = expectedDictionary[expectedKey];
                var actualValue = actualDictionary[expectedKey];

                Assert.AreEqual(expectedValue, actualValue);
            }
        }


        private void AssertMocks()
        {
            _productFactoryMock.VerifyAll();
            _productRepositoryMock.VerifyAll();
            _stockMovementFactory.VerifyAll();
            _stockMovementRepositoryMock.VerifyAll();
        }

        [Test]
        public void AddStock_ValidStock_Success()
        {
            var expectedMovement = MockDataGenerator.Current.GenerateMovementToday();

            SetUpMocksForAdd(default, expectedMovement);

            var stockManager = CreateStockManager();

            var date = expectedMovement.Date;
            var label = expectedMovement.Label;
            var productId = expectedMovement.Product.Id;
            var quantity = expectedMovement.Quantity;

            var result = stockManager.AddMovement(date, label, productId, quantity);

            AssertMovement(expectedMovement, result);
            AssertMocks();
        }

        [Test]
        public void AddMultipleStock_ValidStocks_Success()
        {     
            var label = MockDataGenerator.Current.GenerateLabel(10);
            var dateAfter = MockDataGenerator.Current.GenerateDateAfter(10);
            var productQuantities = MockDataGenerator.Current.GenerateProductDictionary(3);

            var inventories = productQuantities.Select(p => MockDataGenerator.Current.GenerateInventoryToday(p.Key)).ToList();
            var expectedMovements = productQuantities.Select(p => CreateSingleStockMovement(dateAfter, label, p.Key, p.Value)).ToList();

            var inventorieDic = inventories.ToDictionary(i => i.Product.Id);
            _productRepositoryMock.Setup(pr => pr.AddProducts(It.Is<IEnumerable<Product>>(args => AreSame(args, productQuantities.Keys))));
            _productRepositoryMock.Setup(pr => pr.FilterExistingProductIds(productQuantities.Keys)).Returns(Enumerable.Empty<string>());
            _stockMovementRepositoryMock.Setup(smr => smr.GetLatestInventoryMovementsUpToDate(dateAfter, productQuantities.Keys)).Returns(inventories);
            _stockMovementRepositoryMock.Setup(smr => smr.AddMovements(It.Is<IEnumerable<StockMovement>>(args => AreSame(args, expectedMovements)))).Returns(expectedMovements.Count);
            expectedMovements.ForEach(sm => _stockMovementFactory.Setup(sf => sf.GetStock(inventorieDic[sm.Product.Id], sm.Date, label, sm.Product.Id, sm.Quantity)).Returns(sm));

            var stockManager = CreateStockManager();

            var result = stockManager.AddMultipleStock(dateAfter, label, productQuantities);

            AssertMovements(expectedMovements, result);
            AssertMocks();
        }

        [Test]
        public void GetStockForProductAtDate_ValidProductAndDate_ReturnStock()
        {
            var date = DateTime.UtcNow.Date;
            var productId = MockDataGenerator.Current.GenerateProductId();
            var movements = MockDataGenerator.Current.GenerateUniqueMovementsForProductAtDate(productId, date);
            var expectedValue = movements.Sum(sm => sm.Quantity);

            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsForDate(productId, date)).Returns(movements);

            var stockManager = CreateStockManager();

            var result = stockManager.GetStockForProductAtDate(productId, date);

            Assert.AreEqual(expectedValue, result);
            AssertMocks();
        }

        [Test]
        public void GetStockVariationForProduct_ValidProductAndDates_ReturnValue()
        {
            var productId = MockDataGenerator.Current.GenerateProductId();
            var startDate = MockDataGenerator.Current.GenerateDateBefore(5);
            var endDate = MockDataGenerator.Current.GenerateDateAfter(10);

            var movements = new List<StockMovement>()
            {
                CreateSingleStockMovement(startDate,MockDataGenerator.Current.GenerateLabel(10), productId, MockDataGenerator.Current.GenerateQuantity()),
                CreateSingleStockMovement(endDate,MockDataGenerator.Current.GenerateLabel(10), productId, MockDataGenerator.Current.GenerateQuantity()),
            };
            var dictionary = movements.ToDictionary(sm => sm.Date, sm => sm.Quantity);
            var expectedValue = dictionary[endDate] - dictionary[startDate];
            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsBetweenDates(productId, startDate, endDate)).Returns(movements);
            var stockManager = CreateStockManager();

            var result = stockManager.GetStockVariationForProduct(productId, startDate, endDate);

            Assert.AreEqual(expectedValue, result);
            AssertMocks();
        }

        [Test]
        public void GetCurrentStockForProduct_ValidProduct_ReturnValue()
        {
            var productId = MockDataGenerator.Current.GenerateProductId();
            var movements = MockDataGenerator.Current.GenerateUniqueMovementsForProductAtDate(productId, DateTime.UtcNow.Date);
            var expectedValue = movements.Sum(sm => sm.Quantity);

            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsForDate(productId, DateTime.UtcNow.Date)).Returns(movements);

            var stockManager = CreateStockManager();

            var result = stockManager.GetCurrentStockForProduct(productId);

            Assert.AreEqual(expectedValue, result);
            AssertMocks();
        }

        [Test]
        public void GetProductsInStock_WithProducts_Success()
        {
            var movements = new List<StockMovement>()
            {
                CreateSingleStockMovement(DateTime.UtcNow.Date, "Test 1", "EAN12345", 18),
                CreateSingleStockMovement(DateTime.UtcNow.Date.AddDays(-10), "Test 2", "EAN67891", 18),
            };

            var expectedDictionary = movements.ToDictionary(sm => sm.Product.Id, sm => sm.Quantity);
            _stockMovementRepositoryMock.Setup(smr => smr.GetMovementsForDate(DateTime.UtcNow.Date)).Returns(movements);

            var stockManager = CreateStockManager();
            var result = stockManager.GetProductsInStock();

            AssertDictionaries(expectedDictionary, result);
            AssertMocks();
        }

        [Test]
        public void GetTotalProductsInStock_WithProducts_ReturnValue()
        {
            var movements = new List<StockMovement>()
            {
                CreateSingleStockMovement(DateTime.UtcNow.Date, "Test 1", "EAN12345", 10),
                CreateSingleStockMovement(DateTime.UtcNow.Date, "Test 2", "EAN67891", 5),
                CreateSingleStockMovement(DateTime.UtcNow.Date, "Test 2", "EAN67893", 5)
            };
            _stockMovementRepositoryMock.Setup(smr => smr.GetMovementsForDate(DateTime.UtcNow.Date)).Returns(movements);
            
            var stockManager = CreateStockManager();
            var result = stockManager.GetTotalProductsInStock();

            Assert.AreEqual(20, result);
            AssertMocks();
        }

        [Test]
        public void RegularizeStockForProduct_ValidProductAndQuantity_Success()
        {
            var inventory = CreateSingleStockMovement(DateTime.UtcNow.Date,null, "EAN12345", 5);
            var movements = new[]
            {
                CreateSingleStockMovement(DateTime.UtcNow.Date, "Test 3","EAN12345", 15)
            };

            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsForDate(inventory.Product.Id, DateTime.UtcNow.Date)).Returns(movements);
            SetUpMocksForAdd(default, inventory);

            var stockManager = CreateStockManager();

            var productId = inventory.Product.Id;

            stockManager.RegularizeStockForProduct(productId, 20);

            AssertMocks();
        }
    }
}
