using FnacDarty.JobInterview.Stock.Entities;
using System;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public interface IStockMovementFactory
    {
        StockMovement GetStock(StockMovement lastInventory, DateTime date, string productId, long quantity);
        StockMovement GetStock(StockMovement lastInventory, DateTime date, string label, string productId, long quantity);
    }
}
