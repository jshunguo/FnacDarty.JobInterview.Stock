using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Views;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FnacDarty.JobInterview.Stock.UnitTest.Views
{
    [TestFixture]
    internal class ClientTest
    {
        private DateTime _currentDate = new DateTime(2023, 10,12,0,0,0,DateTimeKind.Utc);
        private Mock<IStockManager> _mockStockManager;

        private void AssertGridViewAndMock(string expectedContent, IGridView gridView)
        {
            Assert.IsNotNull(gridView);

            using(var writer = new StringWriter())
            {
                gridView.Render(writer);
                Assert.AreEqual(expectedContent, writer.ToString());
                _mockStockManager.VerifyAll();
            }
        }

        [SetUp]
        public void SetUp() 
        { 
            _mockStockManager = new Mock<IStockManager>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _mockStockManager = null;
        }

        [TestCase("ean12345", 18)]
        public void GetCurrentStockForProduct_ValidProduct_Success(string productId, long quantity)
        {
            _mockStockManager.Setup(sm => sm.GetCurrentStockForProduct(productId)).Returns(quantity);

            var client = new Client(_mockStockManager.Object);

            var gridView = client.GetCurrentStockForProduct(productId);

            AssertGridViewAndMock(Properties.Resources.Expected_GetCurrentStockForProduct, gridView);
        }

        [TestCase("ean12345", 78)]
        public void GetProductsInStock_CurrentDate_Success(string productId, long quantity)
        {
            _mockStockManager.Setup(sm => sm.GetProductsInStock()).Returns(new Dictionary<string, long>
            {
                {productId, quantity} 
            });

            var client = new Client(_mockStockManager.Object);

            var gridView = client.GetProductsInStock();

            AssertGridViewAndMock(Properties.Resources.Expected_GetProductsInStock, gridView);
        }

        [TestCase("ean12345", 45)]
        public void GetStockForProductAtDate_ValidProductAndDate_Success(string productId, long quantity)
        {
            _mockStockManager.Setup(sm => sm.GetStockForProductAtDate(productId, _currentDate)).Returns(quantity);

            var client = new Client(_mockStockManager.Object);

            var gridView = client.GetStockForProductAtDate(productId, _currentDate);

            AssertGridViewAndMock(Properties.Resources.Expected_GetStockForProductAtDate, gridView);
        }

        [TestCase("ean12345", 10, 50)]
        public void GetStockVariationForProduct_ValidProductAndDates_Success(string productId, long quantityStart, long quantityEnd)
        {
            var startDate = _currentDate;
            var endDate = _currentDate.AddDays(3);

            _mockStockManager.Setup(sm => sm.GetStockForProductAtDate(productId, startDate)).Returns(quantityStart);
            _mockStockManager.Setup(sm => sm.GetStockForProductAtDate(productId, endDate)).Returns(quantityEnd);
            _mockStockManager.Setup(sm => sm.GetStockVariationForProduct(productId, startDate, endDate)).Returns(quantityEnd - quantityStart);

            var client = new Client(_mockStockManager.Object);

            var gridView = client.GetStockVariationForProduct(productId, startDate, endDate);

            AssertGridViewAndMock(Properties.Resources.Expected_GetStockVariationForProduct, gridView);
        }

        [TestCase(108)]
        public void GetTotalProductsInStock_CurrentDate_Success(long quantity)
        {
            _mockStockManager.Setup(sm => sm.GetTotalProductsInStock()).Returns(quantity);

            var client = new Client(_mockStockManager.Object);

            var gridView = client.GetTotalProductsInStock();

            AssertGridViewAndMock(Properties.Resources.Expected_GetTotalProductsInStock, gridView);
        }

    }
}
