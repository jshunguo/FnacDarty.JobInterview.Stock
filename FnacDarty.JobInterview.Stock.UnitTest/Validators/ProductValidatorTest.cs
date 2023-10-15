using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Validators;
using NUnit.Framework;
using System;

namespace FnacDarty.JobInterview.Stock.UnitTest.Validators
{
    [TestFixture]
    internal class ProductValidatorTest
    {
        [TestCase("ean12345")]
        public void Validate_ValidProduct_ReturnTrue(string productId)
        {
            var eanValidator = new ProductValidator();
            var product = new Product(productId);

            var result = eanValidator.Validate(product);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Throws);
        }

        [TestCase(null, "L'identifiant du produit  doit comporter 8 charactère alphanumerique")]
        [TestCase("", "L'identifiant du produit  doit comporter 8 charactère alphanumerique")]
        [TestCase("aa11aa55aa99", "L'identifiant du produit aa11aa55aa99 doit comporter 8 charactère alphanumerique")]
        [TestCase("a@i#12aR", "L'identifiant du produit a@i#12aR doit comporter 8 charactère alphanumerique")]
        public void Validate_InvalidProduct_ReturnFalse(string productId, string expectedMessage) 
        {
            var eanValidator = new ProductValidator();
            var product = new Product(productId);

            var result = eanValidator.Validate(product);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.Throws);
            Assert.IsInstanceOf<ArgumentException>(result.Throws);
            Assert.AreEqual(expectedMessage, result.Throws.Message);
        }
    }
}
