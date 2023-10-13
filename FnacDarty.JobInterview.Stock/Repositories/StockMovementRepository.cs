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

        public int AddMovements(IEnumerable<StockMovement> stockMovements)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetProductMovementsBetweenDates(string productId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetMovementsForDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetMovementsForProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetProductMovementsForDate(string productId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public StockMovement GetLatestInventoryMovementForProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StockMovement> GetLatestInventoryMovementsUpToDate(DateTime date, IEnumerable<string> productIds)
        {
            throw new NotImplementedException();
        }
    }
}
