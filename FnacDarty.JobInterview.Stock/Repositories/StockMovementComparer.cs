using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    internal class StockMovementComparer : IComparer<StockMovement>
    {
        public int Compare(StockMovement x, StockMovement y)
        {
            int dateComparison = x.Date.CompareTo(y.Date);
            if (dateComparison != 0) return dateComparison;

            int labelComparison = x.Label.CompareTo(y.Label);
            if (labelComparison != 0) return labelComparison;

            int productComparison = x.Product.Id.CompareTo(y.Product.Id);
            if (productComparison != 0) return productComparison;

            return x.Quantity.CompareTo(y.Quantity);
        }
    }
}
