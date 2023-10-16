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

            int labelComparison = string.Compare(x.Label, y.Label);
            if (labelComparison != 0) return labelComparison;

            int productComparison = string.Compare(x.Product.Id, y.Product.Id);
            if (productComparison != 0) return productComparison;

            return x.Quantity.CompareTo(y.Quantity);
        }
    }


}
