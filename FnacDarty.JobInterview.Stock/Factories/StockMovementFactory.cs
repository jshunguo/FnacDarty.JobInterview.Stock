using FnacDarty.JobInterview.Stock.Entities;
using System;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public class StockMovementFactory : FactoryBase<StockMovement>, IStockMovementFactory
    {
        public StockMovement GetStock(StockMovement lastInventory, DateTime date, string label, string productId, long quantity)
        {
            var stockMovement = string.IsNullOrEmpty(label) ? new StockMovement(date, new Product(productId), quantity) : new StockMovement(date, label, new Product(productId), quantity);

            ValidateAndThrows(stockMovement, stockMovement.GetValidators(lastInventory));

            return stockMovement;
        }

        public StockMovement GetStock(StockMovement lastInventory, DateTime date, string productId, long quantity)
        {
           return GetStock(lastInventory, date,null, productId, quantity);
        }
    }
}
