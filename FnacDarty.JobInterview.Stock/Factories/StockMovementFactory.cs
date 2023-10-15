using FnacDarty.JobInterview.Stock.Entities;
using System;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public class StockMovementFactory : IStockMovementFactory
    {
        public StockMovement Get(StockMovement lastInventory, DateTime date, string label, string productId, long quantity)
        {
            return string.IsNullOrEmpty(label) ? new StockMovement(date, new Product(productId), quantity) : new StockMovement(date, label, new Product(productId), quantity); ;
        }

        public StockMovement Get(StockMovement lastInventory, DateTime date, string productId, long quantity)
        {
           return Get(lastInventory, date,null, productId, quantity);
        }
    }
}
