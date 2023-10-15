using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Factories;
using FnacDarty.JobInterview.Stock.Validators;
using Moq;
using NUnit.Framework;
using System;

namespace FnacDarty.JobInterview.Stock.UnitTest.Factories
{
    [TestFixture]
    public class ValidatingStockMovementFactoryDecoratorTest
    {
        private Mock<IStockMovementFactory> mockInnerFactory;
        private Mock<IValidatorFactory> mockValidatorFactory;
        private Mock<IValidator<StockMovement>> mockValidator;

        [SetUp]
        public void SetUp()
        {
            mockInnerFactory = new Mock<IStockMovementFactory>(MockBehavior.Strict);
            mockValidatorFactory = new Mock<IValidatorFactory>(MockBehavior.Strict);
            mockValidator = new Mock<IValidator<StockMovement>>(MockBehavior.Strict);
        }

        [Test]
        public void Get_FromInnerFactory_RetrieveStockMovement()
        {
            var lastInventory = new StockMovement(DateTime.UtcNow.Date, new Product("ean12345"), 5);
            var stockMovement = new StockMovement(DateTime.UtcNow.AddDays(2).Date, "Test Regular", new Product("ean12345"), 45);

            var date = stockMovement.Date;
            var label = stockMovement.Label;
            var productId = stockMovement.Product.Id;
            var quantity = stockMovement.Quantity;

            mockValidator.Setup(v => v.Validate(stockMovement)).Returns(new ValidatorResult(true, null));
            mockInnerFactory.Setup(f => f.Get(It.Is<StockMovement>(arg => arg.Equals(lastInventory)), 
                                              It.Is<DateTime>(arg => arg.Equals(date)), 
                                              It.Is<string>(arg => arg == label), 
                                              It.Is<string>(arg => arg == productId), 
                                              It.Is<long>(arg => arg == quantity))).Returns(stockMovement);

            mockValidatorFactory.Setup(f => f.GetRegularMovementValidator(It.Is<DateTime>(d => d.Equals(lastInventory.Date)))).Returns(mockValidator.Object);

            var factory = new ValidatingStockMovementFactoryDecorator(mockInnerFactory.Object, mockValidatorFactory.Object);

            var result = factory.Get(lastInventory, stockMovement.Date, stockMovement.Label, stockMovement.Product.Id, stockMovement.Quantity);

            mockInnerFactory.Verify(f => f.Get(It.Is<StockMovement>(arg => arg.Equals(lastInventory)),
                                              It.Is<DateTime>(arg => arg.Equals(date)),
                                              It.Is<string>(arg => arg == label),
                                              It.Is<string>(arg => arg == productId),
                                              It.Is<long>(arg => arg == quantity)), Times.Once);
            Assert.AreEqual(stockMovement, result);
        }

        [Test]
        public void Get_BasedOnIsInventoryValue_UseCorrectValidator()
        {
            var lastInventory = default(StockMovement);

            var inventoryMovement = new StockMovement(DateTime.UtcNow.Date, new Product("ean12345"), 2);
            var date = inventoryMovement.Date;
            var productId = inventoryMovement.Product.Id;
            var quantity = inventoryMovement.Quantity;

            mockInnerFactory.Setup(f => f.Get(It.Is<StockMovement>(arg => arg.Equals(lastInventory)), 
                                              It.Is<DateTime>(arg => arg.Equals(date)), 
                                              It.Is<string>(label => label == null), 
                                              It.Is<string>(product => product == productId), 
                                              It.Is<long>(q => q == quantity))).Returns(inventoryMovement);

            mockValidatorFactory.Setup(f => f.GetInventoryMovementValidator(It.Is<StockMovement>(sm => sm.Equals(lastInventory)))).Returns(mockValidator.Object);
            mockValidator.Setup(v => v.Validate(inventoryMovement)).Returns(new ValidatorResult(true, null));
            var factory = new ValidatingStockMovementFactoryDecorator(mockInnerFactory.Object, mockValidatorFactory.Object);

            factory.Get(lastInventory, date, productId, quantity);

            mockValidator.Verify(v => v.Validate(inventoryMovement), Times.Once);
        }

        [Test]
        public void Get_IfStockMovementIsInvalid_ThrowException()
        {
            var lastInventory = new StockMovement(DateTime.UtcNow.Date, new Product("ean12345"), 5);
            var stockMovement = new StockMovement(DateTime.UtcNow.Date, "Test Regular", new Product("ean12345"), 45);
            var date = stockMovement.Date;
            var label = stockMovement.Label;
            var productId = stockMovement.Product.Id;
            var quantity = stockMovement.Quantity;

            var exception = new Exception("Validation failed");
            var validationResult = new ValidatorResult(false, exception);
            mockValidator.Setup(v => v.Validate(It.Is<StockMovement>(arg => arg.Equals(stockMovement)))).Returns(validationResult);
            mockInnerFactory.Setup(f => f.Get(It.Is<StockMovement>(arg => arg.Equals(lastInventory)), 
                                              It.Is<DateTime>(d => d.Equals(date)), 
                                              It.Is<string>(l => l == label), 
                                              It.Is<string>(p => p == productId), 
                                              It.Is<long>(q => q == quantity))).Returns(stockMovement);

            mockValidatorFactory.Setup(f => f.GetRegularMovementValidator(It.Is<DateTime>(d => d.Equals(lastInventory.Date)))).Returns(mockValidator.Object);

            var factory = new ValidatingStockMovementFactoryDecorator(mockInnerFactory.Object, mockValidatorFactory.Object);

            var thrownException = Assert.Throws<Exception>(() => factory.Get(lastInventory, date, label, productId, quantity));

            Assert.AreSame(exception, thrownException);
        }
    }

}
