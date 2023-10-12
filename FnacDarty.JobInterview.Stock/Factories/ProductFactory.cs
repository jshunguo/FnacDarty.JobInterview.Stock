using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public class ProductFactory : FactoryBase<Product>, IProductFactory
    {
        public Product Get(string productId)
        {
            return Get(productId, 0);
        }

        public Product Get(string productId, long quantity)
        {
            var product = new Product(productId);

            ValidateAndThrows(product, product.GetValidators());

            return product;
        }
    }
}
