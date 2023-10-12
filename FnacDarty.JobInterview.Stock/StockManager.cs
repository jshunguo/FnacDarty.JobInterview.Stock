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

        public StockManager(IStockMovementFactory stockMovementFactory, IProductFactory productFactory, IProductRepository productRepository, IStockMovementRepository stockMovementRepository) 
        { 
            _stockMovementRepository = stockMovementRepository;
            _productFactory = productFactory;
            _productRepository = productRepository;
            _stockMovementFactory = stockMovementFactory;
        }

        private void CreateProduct(string productId)
        {
            var product = _productFactory.Get(productId);
            _productRepository.Add(product);
        }

        private void CreateMovement(DateTime date, string label, string productId, long quantity)
        {
            if (!_productRepository.Exists(productId))
            {
                CreateProduct(productId);
            }

            var lastInventory = _stockMovementRepository.GetLatestInventoryForProduct(productId);

            var stockManagement = _stockMovementFactory.GetStock(lastInventory, date, label, productId, quantity);

            _stockMovementRepository.AddMovement(stockManagement);
        }

        private void AddStockInventory(DateTime date, string productId, long quantity)
        {
            CreateMovement(date, null, productId, quantity);
        }

        public void AddStock(DateTime date, string label, string productId, long quantity)
        {
            CreateMovement(date, label, productId, quantity);
        }

        public void AddMultipleStock(DateTime date, string label, IDictionary<string, long> products)
        {
            if (!_productRepository.Exists(products.Keys))
            {
                products.ForEach(item => CreateProduct(item.Key));
            }

            products.ForEach(product => CreateMovement(date, label, product.Key, product.Value));
        }

        public long GetCurrentStockForProduct(string productId)
        {
            return GetStockForProductAtDate(productId, DateTime.UtcNow.Date);
        }

        public IDictionary<string, long> GetProductsInStock()
        {
            var stocks = _stockMovementRepository.GetByDate(DateTime.UtcNow.Date);

            var currentStocks = new Dictionary<string, long>();

            foreach (var stockMovement in stocks)
            {
                if (!currentStocks.ContainsKey(stockMovement.Product.Id))
                {
                    currentStocks[stockMovement.Product.Id] = 0;
                }

                currentStocks[stockMovement.Product.Id] += stockMovement.Quantity;
            }

            return currentStocks;
        }

        public long GetStockForProductAtDate(string productId, DateTime date)
        {
            var stocks = _stockMovementRepository.GetByProductAndDate(productId, date);

            return stocks.Sum(sm => sm.Quantity);
        }

        public long GetStockVariationForProduct(string productId, DateTime startDate, DateTime endDate)
        {
            var stockStart = GetStockForProductAtDate(productId, startDate);
            var stockEnd = GetStockForProductAtDate(productId, endDate);

            return stockEnd - stockStart;
        }

        public long GetTotalProductsInStock()
        {
            var currentDate = DateTime.UtcNow.Date;

            var stocks = _stockMovementRepository.GetByDate(currentDate);

            return stocks.Sum(sm => sm.Quantity);
        }

        public void RegularizeStockForProduct(string productId, long quantity)
        {
            if(quantity > 0)
            {
                var currentDate = DateTime.UtcNow.Date;

                var stockValue = GetStockForProductAtDate(productId, currentDate);

                if (quantity != stockValue)
                {
                    AddStockInventory(currentDate, productId, quantity - stockValue);
                }
            }
        }
    }
}
