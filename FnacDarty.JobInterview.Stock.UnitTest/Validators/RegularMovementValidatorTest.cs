using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Validators;
using NUnit.Framework;
using System;

namespace FnacDarty.JobInterview.Stock.UnitTest.Validators
{
    [TestFixture]
    internal class RegularMovementValidatorTest
    {
        [Test]
        public void Validate_NullInventoryDate_ReturnTrue()
        {
            var stockMovement = new StockMovement(DateTime.UtcNow, "Un mouvement de produit", new Product("ean12345"), 0);
            var addStockValidator = new RegularMovementValidator(null);

            var result = addStockValidator.Validate(stockMovement);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Throws);
        }

        [Test]
        public void Validate_StockDateGreathanInventoryDate__ReturnTrue()
        {
            var currentDate = DateTime.UtcNow;
            var stockMovement = new StockMovement(currentDate, new Product("ean12345"), 0);
            var addStockValidator = new RegularMovementValidator(currentDate.AddDays(-15));

            var result = addStockValidator.Validate(stockMovement);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Throws);
        }

        [TestCase("[11/10/2023, Un mouvement de produit] : Impossible d'ajouter un mouvement avant la date d'inventaire 11/10/2023")]
        public void Validate_StockDateLowerthanOrEqualsInventoryDate_ReturnFalse(string expectedMessage)
        {
            var currentDate = new DateTime(2023,10,11,0,0,0,DateTimeKind.Utc);
            var stockMovement = new StockMovement(currentDate, "Un mouvement de produit", new Product("ean12345"), 0);
            var addStockValidator = new RegularMovementValidator(currentDate);

            var result = addStockValidator.Validate(stockMovement);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.Throws);
            Assert.IsInstanceOf<ArgumentException>(result.Throws);
            Assert.AreEqual(expectedMessage, result.Throws.Message);
        }
    }
}
