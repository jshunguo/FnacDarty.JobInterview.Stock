using FnacDarty.JobInterview.Stock.Factories;
using NUnit.Framework;
using System;

namespace FnacDarty.JobInterview.Stock.UnitTest.Factories
{
    [TestFixture]
    public class ProductFactoryTest
    {
        [TestCase("ean12345")] 
        public void Get_ValidProducId_ReturnProduct(string productId) 
        {
            var factory = new ProductFactory();

            var result = factory.Get(productId);

            Assert.IsNotNull(result);
            Assert.AreEqual(productId, result.Id);
        }

        [TestCase(null, "L'identifiant du produit  doit comporter 8 charactère alphanumerique")]
        [TestCase("", "L'identifiant du produit  doit comporter 8 charactère alphanumerique")]
        [TestCase("ean1234@", "L'identifiant du produit ean1234@ doit comporter 8 charactère alphanumerique")]
        [TestCase("ean1234@65893j", "(L'identifiant du produit ean1234@65893j doit comporter 8 charactère alphanumerique")]
        public void Get_InvalidProductId_ThrowsError(string productId, string expectedMessage)
        {
            var factory = new ProductFactory();

            Assert.Throws<AggregateException>(() => factory.Get(productId), expectedMessage);
        }
    }
}
