using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Diagnostics.SymbolStore;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    public struct ProductDateKey : IEquatable<ProductDateKey>
    {
        public DateTime Date { get; }
        public string ProductId { get; }

        public ProductDateKey(string productId, DateTime date)
        {
            ProductId = productId;
            Date = date;
        }

        bool IEquatable<ProductDateKey>.Equals(ProductDateKey other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is ProductDateKey other)
            {
                return Date.Equals(other.Date) && ProductId == other.ProductId;
            }
            return false;
        }

        public override int GetHashCode() 
        { 
            return HashCode.Combine(ProductId, Date);  
        }

        public override string ToString() { return $"{ProductId}#{Date}"; }
    }
}
