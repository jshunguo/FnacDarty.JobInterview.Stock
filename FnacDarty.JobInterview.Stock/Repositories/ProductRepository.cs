using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public void Add(Product product)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string productId)
        {
            throw new NotImplementedException();
        }

        public bool Exists(IEnumerable<string> productIds)
        {
            throw new NotImplementedException();
        }

        public Product GetById(string productId)
        {
            throw new NotImplementedException();
        }
    }
}
