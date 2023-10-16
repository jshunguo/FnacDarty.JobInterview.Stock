using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly IDictionary<string, Product> _productDictionary;

        public InMemoryProductRepository(IDictionary<string, Product> productDictionary)
        {
            _productDictionary = productDictionary;
        }

        public InMemoryProductRepository() :this(new ConcurrentDictionary<string, Product>()) { }

        public void AddProduct(Product product)
        {
            if (product.Equals(default)) throw new ArgumentNullException(nameof(product));

            _productDictionary[product.Id] = product;
        }

        public void AddProducts(IEnumerable<Product> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            Parallel.ForEach(products, product =>
            {
                AddProduct(product);
            });
        }

        public Product FindProductById(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) throw new ArgumentNullException(nameof(productId));

            return _productDictionary.TryGetValue(productId, out var product) ? product : default;
        }

        public IEnumerable<string> FilterExistingProductIds(IEnumerable<string> productIds)
        {
            if (productIds == null) throw new ArgumentNullException(nameof(productIds));

            return productIds.Where(_productDictionary.ContainsKey);
        }

        public bool IsProductExisting(string productId)
        {
            return _productDictionary.ContainsKey(productId);
        }
    }
}
