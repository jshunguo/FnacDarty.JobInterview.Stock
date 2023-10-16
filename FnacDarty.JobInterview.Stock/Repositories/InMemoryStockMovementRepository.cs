using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    public class InMemoryStockMovementRepository : IStockMovementRepository
    {
        private readonly IDictionary<string, SortedSet<StockMovement>> _movementsByProduct;
        private readonly IDictionary<DateTime, SortedSet<StockMovement>> _movementsByDate;
        private readonly object _lockObject = new object();

        public InMemoryStockMovementRepository(IDictionary<string, SortedSet<StockMovement>> movementsByProduct,
            IDictionary<DateTime, SortedSet<StockMovement>> movementsByDate)
        {
            _movementsByDate = movementsByDate;
            _movementsByProduct = movementsByProduct;
        }

        public InMemoryStockMovementRepository() : 
            this(new ConcurrentDictionary<string, SortedSet<StockMovement>>(), 
                new ConcurrentDictionary<DateTime, SortedSet<StockMovement>>())
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
            lock (_lockObject)
            {
                GetOrCreateSetForProduct(stockMovement.Product.Id).Add(stockMovement);
                GetOrCreateSetForDate(stockMovement.Date).Add(stockMovement);
            }
        }

        public int AddMovements(IEnumerable<StockMovement> stockMovements)
        {
            var movementsByProduct = stockMovements.GroupBy(m => m.Product.Id);
            var movementsByDate = stockMovements.GroupBy(m => m.Date);

            foreach (var group in movementsByProduct)
            {
                var set = GetOrCreateSetForProduct(group.Key);
                lock (_lockObject)
                {
                    set.UnionWith(group);
                }
            }

            foreach (var group in movementsByDate)
            {
                var set = GetOrCreateSetForDate(group.Key);
                lock (_lockObject)
                {
                    set.UnionWith(group);
                }
            }

            return stockMovements.Count();
        }

        public IEnumerable<StockMovement> GetProductMovementsBetweenDates(string productId, DateTime startDate, DateTime endDate)
        {
            if (_movementsByProduct.TryGetValue(productId, out var set))
            {
                var endMovement = new StockMovement(DateTime.MaxValue, "zzz", new Product(productId), int.MaxValue);
                var startMovement = new StockMovement(DateTime.MinValue, "", new Product(productId), int.MinValue);

                return set.GetViewBetween(startMovement, endMovement);
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

        public StockMovement? GetLatestInventoryMovementForProduct(string productId)
        {
            if (_movementsByProduct.TryGetValue(productId, out var set))
            {
                var endMovement = new StockMovement(DateTime.MaxValue, "zzz", new Product(productId), int.MaxValue);
                var startMovement = new StockMovement(DateTime.MinValue, "", new Product(productId), int.MinValue);

                return set.GetViewBetween(startMovement, endMovement).Reverse().FirstOrDefault(sm => sm.IsInventory);
            }
            return null;
        }

        public IDictionary<string, StockMovement?> GetLatestInventoryMovementsUpToDate(IEnumerable<string> productIds)
        {
            var today = DateTime.UtcNow.Date;
            var earliestDate = DateTime.MinValue.ToUniversalTime().Date;

            return productIds.ToDictionary(
                productId => productId,
                productId => GetLatestInventoryMovementForProduct(productId)
            );
        }
    }
}
