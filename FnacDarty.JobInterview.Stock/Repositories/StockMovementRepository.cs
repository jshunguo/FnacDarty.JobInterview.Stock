using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        public void AddMovement(StockMovement stockMovement)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetByProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetByProductAndDate(string productId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public StockMovement GetLatestInventoryForProduct(string productId)
        {
            throw new NotImplementedException();
        }
    }
}
