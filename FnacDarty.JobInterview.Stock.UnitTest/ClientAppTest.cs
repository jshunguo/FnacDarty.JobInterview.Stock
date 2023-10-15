using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Views;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock
{
    [TestFixture]
    public class ClientAppTest
    {
        private Mock<IStockManager> mockStockManager;
        private Mock<IGridService> mockGridService;

        [SetUp]
        public void SetUp()
        {
            mockStockManager = new Mock<IStockManager>(MockBehavior.Strict);
            mockGridService = new Mock<IGridService>(MockBehavior.Strict);
        }

        private void SetupDisplayDataInGridMock<T>(T data, IReadOnlyList<GridColumn> columns)
        {
            mockGridService.Setup(g => g.DisplayDataInGrid(data, It.Is<IReadOnlyList<GridColumn>>(arg => arg.SequenceEqual(columns))))
                           .Returns(new Mock<IGridView>().Object);
        }

        private void VerifyDisplayDataInGridCalled<T>(T data, IReadOnlyList<GridColumn> columns)
        {
            mockGridService.Verify(g => g.DisplayDataInGrid(data, It.Is<IReadOnlyList<GridColumn>>(arg => arg.SequenceEqual(columns))), Times.Once);
        }

        [Test]
        public void DisplayCurrentStockForProduct_ReturnGridView()
        {
            var productId = "ean12345";
            var columns = new[] { new GridColumn(productId) };
            mockStockManager.Setup(m => m.GetCurrentStockForProduct(productId)).Returns(10L);
            SetupDisplayDataInGridMock(10L, columns);

            var clientApp = new ClientApp(mockStockManager.Object, mockGridService.Object);

            var gridView = clientApp.DisplayCurrentStockForProduct(productId);

            mockStockManager.Verify(m => m.GetCurrentStockForProduct(productId), Times.Once);
            VerifyDisplayDataInGridCalled(10L, columns);
            Assert.NotNull(gridView);
        }

        [Test]
        public void DisplayMovement_WithMovementDetails_ReturnGridView()
        {
            var date = DateTime.UtcNow.Date;
            var label = "Cmd 1";
            var productId = "ean12345";
            var quantity = -10L;

            var mockMovement = new StockMovement(date,label,new Product(productId),quantity);
            var columns = new[] 
            { 
                new GridColumn("Date"),
                new GridColumn("Label"),
                new GridColumn("Quantity"),
                new GridColumn("Product")
            };
            mockStockManager.Setup(m => m.AddMovement(date, label, productId, quantity)).Returns(mockMovement);
            SetupDisplayDataInGridMock(mockMovement, columns);
            var clientApp = new ClientApp(mockStockManager.Object, mockGridService.Object);

            var gridView = clientApp.DisplayMovement(date, label, productId, quantity);

            mockStockManager.Verify(m => m.AddMovement(date, label, productId, quantity), Times.Once);
            VerifyDisplayDataInGridCalled(mockMovement, columns);
            Assert.NotNull(gridView);
        }

        [Test]
        public void DisplayMultipleStockMovements_WithMovements_ReturnGridView()
        {
            var date = DateTime.UtcNow.Date;
            var label = "Achat Fournisseur";
            var productQuantities = new Dictionary<string, long>
            {
                {"ean12345", 10 },
                {"ean67891", 20 }
            };
            var mockMovements = new List<StockMovement>
            {
                new StockMovement(date,label,new Product("ean12345"),10),
                new StockMovement(date,label,new Product("ean67891"),20)
            };
            var columns = new[]
            {
                 new GridColumn("Date"),
                new GridColumn("Label"),
                new GridColumn("Quantity"),
                new GridColumn("Product")
            };

            mockStockManager.Setup(m => m.AddMultipleStock(date, label, productQuantities)).Returns(mockMovements);

            SetupDisplayDataInGridMock(mockMovements, columns);
            var clientApp = new ClientApp(mockStockManager.Object, mockGridService.Object);

            var gridView = clientApp.DisplayMultipleStockMovements(date, label, productQuantities);

            mockStockManager.Verify(m => m.AddMultipleStock(date, label, productQuantities), Times.Once);
            VerifyDisplayDataInGridCalled(mockMovements, columns);
            Assert.NotNull(gridView);
        }

        [Test]
        public void DisplayProductsInStock_WithProductDetails_ReturnGridView()
        {
            var productByQuantityDic = new Dictionary<string, long>
            {
                {"ean12345", 10 },
                {"ean67891", 20 }
            };
            var columns = productByQuantityDic.Keys.Select(k => new GridColumn(k)).ToArray();
            mockStockManager.Setup(m => m.GetProductsInStock()).Returns(productByQuantityDic);
            SetupDisplayDataInGridMock(productByQuantityDic.Values, columns);
            var clientApp = new ClientApp(mockStockManager.Object, mockGridService.Object);

            var gridView = clientApp.DisplayProductsInStock();

            mockStockManager.Verify(m => m.GetProductsInStock(), Times.Once);
            VerifyDisplayDataInGridCalled(productByQuantityDic.Values, columns);
            Assert.NotNull(gridView);
        }

        [Test]
        public void DisplayStockForProductAtDate_WithStockDetails_ReturnGridView()
        {
            var productId = "ean12345";
            var date = DateTime.UtcNow.Date;
            var stockValue = 10L;
            var columns = new[] 
            {
                new GridColumn(string.Empty),
                new GridColumn(productId) 
            };
            var values = new object[] { date.ToShortDateString(), stockValue };

            mockStockManager.Setup(m => m.GetStockForProductAtDate(productId, date)).Returns(stockValue);

            SetupDisplayDataInGridMock<object[]>(values, columns);

            var clientApp = new ClientApp(mockStockManager.Object, mockGridService.Object);

            var gridView = clientApp.DisplayStockForProductAtDate(productId, date);

            mockStockManager.Verify(m => m.GetStockForProductAtDate(productId, date), Times.Once);
            VerifyDisplayDataInGridCalled(values, columns);
            Assert.NotNull(gridView);
        }

        [Test]
        public void DisplayStockVariationForProduct_WithStockVariationDetails_ReturnGridView()
        {
            var productId = "ean12345";
            var startDate = DateTime.UtcNow.AddDays(-5);
            var endDate = DateTime.UtcNow;
            var stockVariationValue = 15L;

            var columns = new[] { 
                new GridColumn(string.Empty),
                new GridColumn(productId)
            };
            var values = new object[] { $"[{startDate.ToShortDateString()}, {endDate.ToShortDateString()}]", stockVariationValue };
            mockStockManager.Setup(m => m.GetStockVariationForProduct(productId, startDate, endDate)).Returns(stockVariationValue);
            SetupDisplayDataInGridMock(values, columns);
            var clientApp = new ClientApp(mockStockManager.Object, mockGridService.Object);

            var gridView = clientApp.DisplayStockVariationForProduct(productId, startDate, endDate);

            mockStockManager.Verify(m => m.GetStockVariationForProduct(productId, startDate, endDate), Times.Once);
            VerifyDisplayDataInGridCalled(values, columns);
            Assert.NotNull(gridView);
        }

        [Test]
        public void DisplayTotalProductsInStock_WithTotalProductDetails_ReturnGridView()
        {
            var totalProductsValue = 20L;
            var columns = new[] {
                new GridColumn("Total")
            };
            mockStockManager.Setup(m => m.GetTotalProductsInStock()).Returns(totalProductsValue);
            SetupDisplayDataInGridMock(totalProductsValue, columns);
            var clientApp = new ClientApp(mockStockManager.Object, mockGridService.Object);

            var gridView = clientApp.DisplayTotalProductsInStock();

            mockStockManager.Verify(m => m.GetTotalProductsInStock(), Times.Once);
            VerifyDisplayDataInGridCalled(totalProductsValue, columns);
            Assert.NotNull(gridView);
        }
    }
}
