using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Factories;
using NUnit.Framework;
using System;

namespace FnacDarty.JobInterview.Stock.UnitTest.Factories
{
    [TestFixture]
    public class StockMovementFactoryTest
    {
        private static DateTime _lastInventoryDate = new DateTime(2023, 11, 3, 0, 0, 0, DateTimeKind.Utc);
        private static string productId = "ean12345";

        private StockMovement _lastStockInventory;
        private StockMovementFactory _factory;

        [OneTimeSetUp]
        public void SetupParameters()
        {
            _lastStockInventory = new StockMovement(_lastInventoryDate, new Product(productId), 54);
            _factory = new StockMovementFactory();
        }

        [TestCase("[Inventory:03/11/2023] : Il existe déjà un mouvement d'inventaire pour ce produit ean12345.")]
        public void Get_ExistingInvatory_ThrowsError(string expectedMessage)
        {
            Assert.Throws<AggregateException>(() => _factory.GetStock(_lastStockInventory, _lastInventoryDate, _lastStockInventory.Product.Id, 450), expectedMessage);
        }

        [TestCase("[14/10/2023, Cmd client1] : Impossible d'ajouter un mouvement avant la date d'inventaire 03/11/2023")]
        public void Get_BeforeInventoryDate_ThrowsError(string expectedMessage)
        {
            Assert.Throws<AggregateException>(() => _factory.GetStock(_lastStockInventory, _lastStockInventory.Date.AddDays(-20),"Cmd client1", _lastStockInventory.Product.Id, 450), expectedMessage);
        }

        [Test]
        public void Get_AfterInventoryDate_ReturnsStockMovement()
        {
            var stock = _factory.GetStock(_lastStockInventory, _lastInventoryDate.AddDays(20), "Cmd client1", _lastStockInventory.Product.Id, 450);

            Assert.IsNotNull(stock);
            Assert.Less(_lastInventoryDate, stock.Date);
        }
    }
}
