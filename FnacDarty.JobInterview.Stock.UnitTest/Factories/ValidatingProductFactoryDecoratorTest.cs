using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Factories;
using FnacDarty.JobInterview.Stock.Validators;
using Moq;
using NUnit.Framework;
using System;

namespace FnacDarty.JobInterview.Stock.UnitTest.Factories
{
    [TestFixture]
    public class ValidatingProductFactoryDecoratorTest
    {
        private Mock<IProductFactory> mockInnerFactory;
        private Mock<IValidatorFactory> mockValidatorFactory;
        private Mock<IValidator<Product>> mockValidator;
        private ValidatingProductFactoryDecorator factory;

        [SetUp]
        public void SetUp()
        {
            mockInnerFactory = new Mock<IProductFactory>(MockBehavior.Strict);
            mockValidatorFactory = new Mock<IValidatorFactory>(MockBehavior.Strict);
            mockValidator = new Mock<IValidator<Product>>(MockBehavior.Strict);

            mockValidatorFactory.Setup(f => f.GetProductValidator()).Returns(mockValidator.Object);
            factory = new ValidatingProductFactoryDecorator(mockInnerFactory.Object, mockValidatorFactory.Object);
        }

        [Test]
        public void Get_FromInnerFactory_RetrieveProduct()
        {
            var productId = "ean12345";
            var expectedProduct = new Product(productId);
            mockInnerFactory.Setup(f => f.Get(productId)).Returns(expectedProduct);
            mockValidator.Setup(v => v.Validate(It.Is<Product>(p => p.Id == productId))).Returns(new ValidatorResult(true, null));

            var result = factory.Get(productId);

            mockInnerFactory.Verify(f => f.Get(productId), Times.Once);
            Assert.AreEqual(expectedProduct, result);
        }

        [Test]
        public void Get_ToValidateProduct_UseValidator()
        {
            var productId = "ean67891";
            var expectedProduct = new Product(productId);
            mockInnerFactory.Setup(f => f.Get(productId)).Returns(expectedProduct);
            mockValidator.Setup(v => v.Validate(It.Is<Product>(p => p.Id == productId))).Returns(new ValidatorResult(true, null));

            factory.Get(productId);

            mockValidator.Verify(v => v.Validate(expectedProduct), Times.Once);
        }

        [Test]
        public void Get_IfProductIsInvalid_ThrowException()
        {
            var productId = "ean6789195";
            var exception = new Exception("Validation failed");
            var validationResult = new ValidatorResult(false, exception);
            mockValidator.Setup(v => v.Validate(It.Is<Product>(p => p.Id == productId))).Returns(validationResult);
            mockInnerFactory.Setup(f => f.Get(productId)).Returns(new Product(productId));

            var thrownException = Assert.Throws<Exception>(() => factory.Get(productId));
            Assert.AreSame(exception, thrownException);
        }
    }

}
