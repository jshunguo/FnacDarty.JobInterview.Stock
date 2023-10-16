using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Factories;
using FnacDarty.JobInterview.Stock.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock
{
    public class StockManager : IStockManager
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IProductFactory _productFactory;
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementFactory _stockMovementFactory;

        public StockManager(IProductFactory productFactory, IStockMovementFactory stockMovementFactory, IProductRepository productRepository, IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _productFactory = productFactory;
            _productRepository = productRepository;
            _stockMovementFactory = stockMovementFactory;
        }

        private void CreateProduct(string productId)
        {
            var product = _productFactory.Get(productId);
            _productRepository.AddProduct(product);
        }

        private StockMovement CreateMovement(DateTime date, string label, string productId, long quantity)
        {
            if (!_productRepository.IsProductExisting(productId))
            {
                CreateProduct(productId);
            }

            var lastInventory = _stockMovementRepository.GetLatestInventoryMovementForProduct(productId);

            var stockMovement = _stockMovementFactory.Get(lastInventory, date, label, productId, quantity);

            return stockMovement;
        }

        private IReadOnlyCollection<StockMovement> CreateMovements(DateTime date, string label, IDictionary<string, long> productQuantities)
        {
            var inventoryByProduct = _stockMovementRepository.GetLatestInventoryMovementsUpToDate(productQuantities.Keys);
            var result = new List<StockMovement>();

            bool inventoriesIsEmpty = inventoryByProduct.Count == 0;

            foreach (var productQuantity in productQuantities)
            {
                if (inventoriesIsEmpty)
                {
                    result.Add(_stockMovementFactory.Get(default, date, label, productQuantity.Key, productQuantity.Value));
                }
                else if (inventoryByProduct.TryGetValue(productQuantity.Key, out var existingInventory))
                {
                    result.Add(_stockMovementFactory.Get(existingInventory, date, label, productQuantity.Key, productQuantity.Value));
                }
            }

            return result;
        }

        public StockMovement AddMovement(DateTime date, string label, string productId, long quantity)
        {
            var stock = CreateMovement(date, label, productId, quantity);
            _stockMovementRepository.AddMovement(stock);
            return stock;
        }

        public IReadOnlyCollection<StockMovement> AddMultipleStock(DateTime date, string label, IDictionary<string, long> products)
        {
            var existingProductIds = _productRepository.FilterExistingProductIds(products.Keys);
            var existingProductSet = new HashSet<string>(existingProductIds);
            var currentProductSet = new HashSet<string>(products.Keys);

            var missingProducts = currentProductSet.Except(existingProductSet).ToList();

            if (missingProducts.Count > 0)
            {
                _productRepository.AddProducts(missingProducts.Select(p => new Product(p)));
            }

            var stocks = CreateMovements(date, label, products);

            if (stocks.Count > 0)
            {
                _stockMovementRepository.AddMovements(stocks);
            }

            return stocks;
        }

        public long GetCurrentStockForProduct(string productId)
        {
            return GetStockForProductAtDate(productId, DateTime.UtcNow.Date);
        }

        public IDictionary<string, long> GetProductsInStock()
        {
            var movements = _stockMovementRepository.GetMovementsForDate(DateTime.UtcNow.Date);
            var productByQuantityDictionary = new Dictionary<string, long>();

            foreach (var movement in movements)
            {
                if (!productByQuantityDictionary.ContainsKey(movement.Product.Id))
                {
                    productByQuantityDictionary[movement.Product.Id] = 0;
                }

                productByQuantityDictionary[movement.Product.Id] += movement.Quantity;
            }

            return productByQuantityDictionary;
        }

        public long GetStockForProductAtDate(string productId, DateTime date)
        {
            var stocks = _stockMovementRepository.GetProductMovementsForDate(productId, date);
            return stocks.Sum(sm => sm.Quantity);
        }

        public long GetStockVariationForProduct(string productId, DateTime startDate, DateTime endDate)
        {
            var movements = _stockMovementRepository.GetProductMovementsBetweenDates(productId, startDate, endDate);
            long startQuantity = 0;
            long endQuantity = 0;

            foreach (var movement in movements)
            {
                if (movement.Date == startDate)
                {
                    startQuantity += movement.Quantity;
                }
                else if (movement.Date == endDate)
                {
                    endQuantity += movement.Quantity;
                }
            }

            return endQuantity - startQuantity;
        }

        public long GetTotalProductsInStock()
        {
            var currentDate = DateTime.UtcNow.Date;

            var stocks = _stockMovementRepository.GetMovementsForDate(currentDate);

            return stocks.Sum(sm => sm.Quantity);
        }

        public void RegularizeStockForProduct(string productId, long quantity)
        {
            if(quantity < 0)
            {
                throw new InvalidOperationException();
            }

            var currentDate = DateTime.UtcNow.Date;

            var stockValue = GetStockForProductAtDate(productId, currentDate);

            if (quantity == stockValue)
            {
                throw new InvalidOperationException();
            }

            var stock = CreateMovement(currentDate, StockMovement.InventoryName, productId, quantity - stockValue);

            _stockMovementRepository.AddMovement(stock);
        }
    }
}
