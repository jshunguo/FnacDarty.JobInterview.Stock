using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Validators;
using NUnit.Framework;
using System;

namespace FnacDarty.JobInterview.Stock.UnitTest.Validators
{
    [TestFixture]
    internal class InventoryMovementValidatorTest
    {
        private static DateTime _currentDate = new DateTime(2023,11,3,0,0,0,DateTimeKind.Utc);

        [Test]
        public void Validate_ValidInventory_ReturnTrue()
        {
            var lastInventory = CreateInvantory(42);
            var currentInventory = new StockMovement(DateTime.UtcNow.AddDays(23), lastInventory.Product,0);
            var inventoryValidator = new InventoryMovementValidator(lastInventory);

            var result = inventoryValidator.Validate(currentInventory);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Throws);
        }

        [TestCase(-15, "[Inventory:03/11/2023] : Les mouvements d'inventaire ne peuvent pas avoir une quantité négative ou null.")]
        [TestCase(42, "[Inventory:03/11/2023] : Il existe déjà un mouvement d'inventaire pour ce produit ean12345.")]
        public void Validate_InvalidInventory_ReturnFalse(long quantity, string expectedMessage)
        {
            var lastInventory = CreateInvantory(quantity);
            var currentInventory = new StockMovement(lastInventory.Date, new Product(lastInventory.Product.Id), quantity);
            var inventoryValidator = new InventoryMovementValidator(lastInventory);

            var result = inventoryValidator.Validate(currentInventory);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.Throws);
            Assert.IsInstanceOf<ArgumentException>(result.Throws);
            Assert.AreEqual(expectedMessage, result.Throws.Message);
        }

        private static StockMovement CreateInvantory(long quantity)
        {
            var product = new Product("ean12345");

            return new StockMovement(_currentDate, product, quantity);
        }
    }
}
