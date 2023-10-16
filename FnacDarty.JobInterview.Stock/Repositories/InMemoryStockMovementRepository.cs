using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    public class InMemoryStockMovementRepository : IStockMovementRepository
    {
        private readonly IDictionary<string, SortedSet<StockMovement>> _movementsByProduct;
        private readonly IDictionary<DateTime, SortedSet<StockMovement>> _movementsByDate;

        public InMemoryStockMovementRepository(IDictionary<string, SortedSet<StockMovement>> movementsByProduct,
            IDictionary<DateTime, SortedSet<StockMovement>> movementsByDate)
        {
            _movementsByDate = movementsByDate;
            _movementsByProduct = movementsByProduct;
        }

        public InMemoryStockMovementRepository() : 
            this(new Dictionary<string, SortedSet<StockMovement>>(), 
                new Dictionary<DateTime, SortedSet<StockMovement>>())
        {

        }

        private SortedSet<StockMovement> GetOrCreateSetForProduct(string productId)
        {
            if (!_movementsByProduct.TryGetValue(productId, out var set))
            {
                set = new SortedSet<StockMovement>(new StockMovementComparer());
                _movementsByProduct[productId] = set;
            }
            return set;
        }

        private SortedSet<StockMovement> GetOrCreateSetForDate(DateTime date)
        {
            if (!_movementsByDate.TryGetValue(date, out var set))
            {
                set = new SortedSet<StockMovement>(new StockMovementComparer());
                _movementsByDate[date] = set;
            }
            return set;
        }

        public void AddMovement(StockMovement stockMovement)
        {
            GetOrCreateSetForProduct(stockMovement.Product.Id).Add(stockMovement);
            GetOrCreateSetForDate(stockMovement.Date).Add(stockMovement);
        }

        public int AddMovements(IEnumerable<StockMovement> stockMovements)
        {
            int addedCount = 0;
            foreach (var movement in stockMovements)
            {
                AddMovement(movement);
                addedCount++;
            }
            return addedCount;
        }

        public IEnumerable<StockMovement> GetProductMovementsBetweenDates(string productId, DateTime startDate, DateTime endDate)
        {
            if (_movementsByProduct.TryGetValue(productId, out var set))
            {
                return set.Where(sm => sm.Date >= startDate && sm.Date <= endDate);
            }
            return Enumerable.Empty<StockMovement>();
        }

        public IEnumerable<StockMovement> GetMovementsForDate(DateTime date)
        {
            return _movementsByDate.TryGetValue(date, out var set) ? set : Enumerable.Empty<StockMovement>();
        }

        public IEnumerable<StockMovement> GetMovementsForProduct(string productId)
        {
            return _movementsByProduct.TryGetValue(productId, out var set) ? set : Enumerable.Empty<StockMovement>();
        }

        public IEnumerable<StockMovement> GetProductMovementsForDate(string productId, DateTime date)
        {
            return _movementsByDate.TryGetValue(date, out var set)
            ? set.Where(sm => sm.Product.Id == productId)
            : Enumerable.Empty<StockMovement>(); ;
        }

        public StockMovement GetLatestInventoryMovementForProduct(string productId)
        {
            if (_movementsByProduct.TryGetValue(productId, out var set))
            {
                return set.LastOrDefault(sm => sm.IsInventory);
            }
            return default;
        }

        public IDictionary<string, StockMovement> GetLatestInventoryMovementsUpToDate(IEnumerable<string> productIds)
        {
            return productIds.ToDictionary(p => p, p => GetLatestInventoryMovementForProduct(p));
        }
    }
}
