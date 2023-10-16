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

            return string.Compare(x.Product.Id, y.Product.Id);
        }
    }

}
