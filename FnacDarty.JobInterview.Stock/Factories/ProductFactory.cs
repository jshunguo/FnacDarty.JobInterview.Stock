using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public class ProductFactory : IProductFactory
    {
        public Product Get(string productId)
        {
            return new Product(productId);
        }
    }
}
