using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Repositories;
using NUnit.Framework;
using System;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.UnitTest.Repositories
{
    [TestFixture]
    public class InMemoryStockMovementRepositoryTest
    {
        private InMemoryStockMovementRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new InMemoryStockMovementRepository();
        }

        [Test]
        public void AddMovement_ValidMovement_MovementAdded()
        {
            var date = DateTime.UtcNow.Date;

            var movement = new StockMovement(date, "Test", new Product("EAN123"), 16);

            _repository.AddMovement(movement);

            var movementsForProduct = _repository.GetMovementsForProduct("EAN123").ToList();
            var movementsForDate = _repository.GetMovementsForDate(date).ToList();

            Assert.AreEqual(1, movementsForProduct.Count);
            Assert.AreEqual(movement, movementsForProduct[0]);
            Assert.AreEqual(1, movementsForDate.Count);
            Assert.AreEqual(movement, movementsForDate[0]);
        }

        [Test]
        public void AddMovements_MultipleMovements_MovementsAdded()
        {
            var date = DateTime.UtcNow.Date;

            var movement1 = new StockMovement(date, "Cmd 1", new Product("EAN123"), 18);
            var movement2 = new StockMovement(date, "Cmd 2", new Product("EAN456"), 19);

            _repository.AddMovements(new[] { movement1, movement2 });

            Assert.AreEqual(movement1, _repository.GetMovementsForProduct("EAN123").Single());
            Assert.AreEqual(movement2, _repository.GetMovementsForProduct("EAN456").Single());
        }

        [Test]
        public void GetProductMovementsBetweenDates_ValidDates_ReturnsMovements()
        {
            var startDate = DateTime.UtcNow.Date;
            var endDate = startDate.AddDays(5);

            var movement1 = new StockMovement(startDate, "First", new Product("EAN123"), 25);
            var movement2 = new StockMovement(endDate, "Last", new Product("EAN123"), 45);

            _repository.AddMovement(movement1);
            _repository.AddMovement(movement2);

            var movements = _repository.GetProductMovementsBetweenDates("EAN123", startDate, endDate).ToList();

            Assert.AreEqual(2, movements.Count);
        }

        [Test]
        public void GetMovementsForProduct_ValidProductId_ReturnsMovements()
        {
            var date = DateTime.UtcNow.Date;

            var movement = new StockMovement(date, "Test 1", new Product("EAN123"), 16);

            _repository.AddMovement(movement);

            var result = _repository.GetMovementsForProduct("EAN123");

            Assert.AreEqual(movement, result.Single());
        }

        [Test]
        public void GetLatestInventoryMovementForProduct_WithInventoryMovements_ReturnsLatest()
        {
            var date1 = DateTime.UtcNow.Date;
            var date2 = date1.AddDays(3);

            var movement1 = new StockMovement(date1,new Product("EAN123"), 25);
            var movement2 = new StockMovement(date2, new Product("EAN123"), 36);

            _repository.AddMovement(movement1);
            _repository.AddMovement(movement2);

            var latest = _repository.GetLatestInventoryMovementForProduct("EAN123");

            Assert.AreEqual(movement2, latest);
        }

        [Test]
        public void GetProductMovementsForDate_ValidProductIdAndDate_ReturnsMovements()
        {
            var date = DateTime.UtcNow.Date;

            var movement = new StockMovement(date, "Livraison Fournisseur", new Product("EAN123"), 5);
            _repository.AddMovement(movement);

            var result = _repository.GetProductMovementsForDate("EAN123", date);

            Assert.AreEqual(movement, result.Single());
        }

        [Test]
        public void GetLatestInventoryMovementsUpToDate_ReturnsLatestInventoryMovements()
        {
            var date1 = DateTime.UtcNow.Date;
            var date2 = date1.AddDays(2);

            var movement1 = new StockMovement(date1, new Product("EAN123"), 12);
            var movement2 = new StockMovement(date2, new Product("EAN153"), 13);
            var movement3 = new StockMovement(date2, new Product("EAN456"), 15);

            _repository.AddMovement(movement1);
            _repository.AddMovement(movement2);
            _repository.AddMovement(movement3);

            var result = _repository.GetLatestInventoryMovementsUpToDate(new[] { "EAN123", "EAN456" }).ToList();

            Assert.Contains(movement1, result);  
            Assert.Contains(movement3, result);  
        }

        [Test]
        public void GetMovementsForDate_ValidDate_ReturnsMovements()
        {
            var date = DateTime.UtcNow.Date;

            var movement = new StockMovement(date, "Livraison Fournisseur", new Product("EAN123"), 2);
            _repository.AddMovement(movement);

            var result = _repository.GetMovementsForDate(DateTime.Today);

            Assert.AreEqual(movement, result.Single());
        }

        [Test]
        public void GetProductMovementsForDate_InvalidProductId_ReturnsEmpty()
        {
            var result = _repository.GetProductMovementsForDate("INVALID_EAN", DateTime.Today);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetMovementsForDate_InvalidDate_ReturnsEmpty()
        {
            var result = _repository.GetMovementsForDate(DateTime.Today.AddDays(1));  
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetLatestInventoryMovementForProduct_NoInventoryMovements_ReturnsDefault()
        {
            var result = _repository.GetLatestInventoryMovementForProduct("EAN123");
            var expected = default(StockMovement);
            Assert.AreEqual(expected, result);
        }
    }
}
