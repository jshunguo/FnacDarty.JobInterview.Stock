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
        private Mock<IProductFactory> _productFactoryMock;
        private Mock<IStockMovementFactory> _stockMovementFactory;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IStockMovementRepository> _stockMovementRepositoryMock;
        private IStockManager _stockManager;

        [SetUp]
        public void SetUp()
        {
            _productFactoryMock = new Mock<IProductFactory>(MockBehavior.Strict);
            _stockMovementFactory = new Mock<IStockMovementFactory>(MockBehavior.Strict);
            _productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
            _stockMovementRepositoryMock = new Mock<IStockMovementRepository>(MockBehavior.Strict);

            _stockManager = new StockManager(_productFactoryMock.Object, _stockMovementFactory.Object, _productRepositoryMock.Object, _stockMovementRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _productFactoryMock.VerifyAll();
            _stockMovementFactory.VerifyAll();
            _productRepositoryMock.VerifyAll();
            _stockMovementRepositoryMock.VerifyAll();
        }

        private StockMovement GenerateExpectedMovement()
        {
            return MockDataGenerator.Current.GenerateMovementToday();
        }

        private static StockMovement CreateSingleStockMovement(DateTime dateAfter, string label, string productId, long quantity)
        {
            return new StockMovement(dateAfter, label, new Product(productId), quantity);
        }

        private (DateTime, string, IDictionary<string, long>, List<StockMovement>) GenerateMultipleStockData()
        {
            var label = MockDataGenerator.Current.GenerateLabel(10);
            var dateAfter = MockDataGenerator.Current.GenerateDateAfter(10);
            var productQuantities = MockDataGenerator.Current.GenerateProductDictionary(3);
            var expectedMovements = productQuantities
                .Select(p => CreateSingleStockMovement(dateAfter, label, p.Key, p.Value)).ToList();
            return (dateAfter, label, productQuantities, expectedMovements);
        }

        private void SetUpMocksForAdd(StockMovement stockMovement, StockMovement lastInventory = default)
        {
            var productId = stockMovement.Product.Id;
            _productFactoryMock.Setup(pf => pf.Get(productId)).Returns(new Product(productId));
            _productRepositoryMock.Setup(pr => pr.IsProductExisting(productId)).Returns(false);
            _productRepositoryMock.Setup(pr => pr.AddProduct(It.Is<Product>(p => p.Id == productId)));

            if (!lastInventory.Equals(default))
            {
                _stockMovementRepositoryMock.Setup(sr => sr.GetLatestInventoryMovementForProduct(productId)).Returns(lastInventory);
            }

            _stockMovementFactory.Setup(sf => sf.Get(lastInventory, stockMovement.Date, stockMovement.Label, productId, stockMovement.Quantity)).Returns(stockMovement);
            _stockMovementRepositoryMock.Setup(sr => sr.AddMovement(stockMovement));
        }

        private void SetUpMocksForAddMultiple(DateTime dateAfter, string label,
                                                 IDictionary<string, long> productQuantities,
                                                 List<StockMovement> expectedMovements)
        {
            var inventories = productQuantities.Select(p => MockDataGenerator.Current.GenerateInventoryToday(p.Key)).ToList();
            var inventorieDic = inventories.ToDictionary(i => i.Product.Id, i=> (StockMovement?)i);

            _productRepositoryMock.Setup(pr => pr.AddProducts(It.Is<IEnumerable<Product>>(args => AreSame(args, productQuantities.Keys))));
            _productRepositoryMock.Setup(pr => pr.FilterExistingProductIds(productQuantities.Keys)).Returns(Enumerable.Empty<string>());
            _stockMovementRepositoryMock.Setup(smr => smr.GetLatestInventoryMovementsUpToDate(productQuantities.Keys)).Returns(inventorieDic);
            _stockMovementRepositoryMock.Setup(smr => smr.AddMovements(It.Is<IEnumerable<StockMovement>>(args => AreSame(args, expectedMovements)))).Returns(expectedMovements.Count);

            expectedMovements.ForEach(sm => _stockMovementFactory.Setup(sf => sf.Get(inventorieDic[sm.Product.Id], sm.Date, label, sm.Product.Id, sm.Quantity)).Returns(sm));
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
            Assert.That(actualMovement, Is.Not.Null);
            Assert.That(actualMovement.Date, Is.EqualTo(expectedMovement.Date));
            Assert.That(actualMovement.Label, Is.EqualTo(expectedMovement.Label));
            Assert.That(actualMovement.Product, Is.EqualTo(expectedMovement.Product));
            Assert.That(actualMovement.Quantity, Is.EqualTo(expectedMovement.Quantity));
        }

        private static void AssertMovements(IEnumerable<StockMovement> expectedMovements, IEnumerable<StockMovement> actualMovements)
        {
            Assert.That(actualMovements, Is.EquivalentTo(expectedMovements));
        }

        private static void AssertDictionaries(IDictionary<string, long> expectedDictionary, IDictionary<string, long> actualDictionary)
        {
            Assert.That(actualDictionary, Is.EquivalentTo(expectedDictionary));
        }

        [Test]
        public void AddMovement_ValidStock_ReturnExpectedStockMovement()
        {
            var expectedMovement = MockDataGenerator.Current.GenerateMovementToday();
            SetUpMocksForAdd(expectedMovement, StockMovement.DefaultInventory);
            var result = _stockManager.AddMovement(expectedMovement.Date, expectedMovement.Label, expectedMovement.Product.Id, expectedMovement.Quantity);
            AssertMovement(expectedMovement, result);
        }

        [Test]
        public void AddMultipleStock_ValidStocks_ShouldReturnExpectedStockMovements()
        {
            var (dateAfter, label, productQuantities, expectedMovements) = GenerateMultipleStockData();
            SetUpMocksForAddMultiple(dateAfter, label, productQuantities, expectedMovements);

            var result = _stockManager.AddMultipleStock(dateAfter, label, productQuantities);

            AssertMovements(expectedMovements, result);
        }

        [Test]
        public void GetStockForProductAtDate_ValidProductAndDate_ReturnStock()
        {
            var date = DateTime.UtcNow.Date;
            var productId = MockDataGenerator.Current.GenerateProductId();
            var movements = MockDataGenerator.Current.GenerateUniqueMovementsForProductAtDate(productId, date);

            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsForDate(productId, date)).Returns(movements);

            var result = _stockManager.GetStockForProductAtDate(productId, date);

            Assert.AreEqual(movements.Sum(sm => sm.Quantity), result);
        }

        [Test]
        public void GetStockVariationForProduct_ValidProductAndDates_ReturnValue()
        {
            var productId = MockDataGenerator.Current.GenerateProductId();
            var startDate = MockDataGenerator.Current.GenerateDateBefore(5);
            var endDate = MockDataGenerator.Current.GenerateDateAfter(10);

            var movements = MockDataGenerator.Current.GenerateProductMovementsBetweenDates(productId, startDate, endDate);
            var startValue = movements.Where(sm => sm.Date == startDate).Sum(sm => sm.Quantity);
            var endValue = movements.Where(sm => sm.Date == endDate).Sum(sm => sm.Quantity);
            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsBetweenDates(productId, startDate, endDate)).Returns(movements);

            var result = _stockManager.GetStockVariationForProduct(productId, startDate, endDate);

            Assert.AreEqual(endValue - startValue, result);
        }

        [Test]
        public void GetCurrentStockForProduct_ValidProduct_ReturnValue()
        {
            var productId = MockDataGenerator.Current.GenerateProductId();
            var movements = MockDataGenerator.Current.GenerateUniqueMovementsForProductAtDate(productId, DateTime.UtcNow.Date);

            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsForDate(productId, DateTime.UtcNow.Date)).Returns(movements);

            var result = _stockManager.GetCurrentStockForProduct(productId);

            Assert.AreEqual(movements.Sum(sm => sm.Quantity), result);
        }

        [Test]
        public void GetProductsInStock_WithProducts_ReturnProductQuantities()
        {
            var date = DateTime.UtcNow.Date;
            var movements = MockDataGenerator.Current.GenerateMovementsForDate(date);
            _stockMovementRepositoryMock.Setup(smr => smr.GetMovementsForDate(date)).Returns(movements);

            var result = _stockManager.GetProductsInStock();

            var expected = movements.GroupBy(m => m.Product.Id).ToDictionary(g => g.Key, g => g.Sum(m => m.Quantity));

            AssertDictionaries(expected, result);
        }

        [Test]
        public void GetTotalProductsInStock_WithProducts_ReturnValue()
        {
            var date = DateTime.UtcNow.Date;
            var movements = MockDataGenerator.Current.GenerateMovementsForDate(date);
            _stockMovementRepositoryMock.Setup(smr => smr.GetMovementsForDate(DateTime.UtcNow.Date)).Returns(movements);

            var result = _stockManager.GetTotalProductsInStock();

            Assert.AreEqual(movements.Sum(sm => sm.Quantity), result);
        }

        [Test]
        public void RegularizeStockForProduct_ValidProductAndQuantity_Success()
        {
            var productId = "EAN12345";
            var today = DateTime.UtcNow.Date;
            var inventoryQuantity = MockDataGenerator.Current.GenerateQuantity();

            var movement = new StockMovement(today, new Product(productId), MockDataGenerator.Current.GenerateQuantity());
            var expectedMovement = new StockMovement(today, new Product(productId), inventoryQuantity - movement.Quantity);

            SetUpMocksForAdd(expectedMovement, StockMovement.DefaultInventory);

            _stockMovementRepositoryMock.Setup(smr => smr.GetProductMovementsForDate(productId, today)).Returns(new List<StockMovement> { movement });

            Assert.DoesNotThrow(() => _stockManager.RegularizeStockForProduct(productId, inventoryQuantity));
        }
    }
}
