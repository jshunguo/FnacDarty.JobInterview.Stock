using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void AddProducts(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }

        public bool IsProductExisting(string productId)
        {
            throw new NotImplementedException();
        }

        public Product FindProductById(string productId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> FilterExistingProductIds(IEnumerable<string> productIds)
        {
            throw new NotImplementedException();
        }
    }
}
