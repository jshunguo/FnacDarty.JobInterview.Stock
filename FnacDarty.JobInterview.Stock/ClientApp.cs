using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock
{
    public sealed class ClientApp : IClient
    {
        private readonly IStockManager _stockManager;
        private readonly IGridService _gridService;
        public ClientApp(IStockManager stockmanager, IGridService gridService)
        {
            _stockManager = stockmanager;
            _gridService = gridService;
        }

        public IGridView DisplayCurrentStockForProduct(string productId)
        {
            var value = _stockManager.GetCurrentStockForProduct(productId);
            return _gridService.DisplayDataInGrid(value, GetColumnsForForProductStock(productId));
        }

        public IGridView DisplayMovement(DateTime date, string label, string productId, long quantity)
        {
            var movement = _stockManager.AddMovement(date, label, productId, quantity);
            return _gridService.DisplayDataInGrid(movement, GetColumnsForMovementStock());
        }

        public IGridView DisplayMultipleStockMovements(DateTime date, string label, IDictionary<string, long> productQuantities)
        {
            var movements = _stockManager.AddMultipleStock(date, label, productQuantities);
            return _gridService.DisplayDataInGrid(movements, GetColumnsForMovementStock());
        }

        public IGridView DisplayProductsInStock()
        {
            var productByQuantityDic = _stockManager.GetProductsInStock();
            return _gridService.DisplayDataInGrid(productByQuantityDic.Values, GetColumnsForForAllProductStocks(productByQuantityDic.Keys));
        }

        public IGridView DisplayStockAfterRegularizationForProduct(string productId, long quantity)
        {
            _stockManager.RegularizeStockForProduct(productId, quantity);
            var currentStock = new Dictionary<string, long>
            {
                { productId, _stockManager.GetCurrentStockForProduct(productId) }
            };

            return _gridService.DisplayDataInGrid(currentStock.Values, GetColumnsForForAllProductStocks(currentStock.Keys));
        }

        public IGridView DisplayStockForProductAtDate(string productId, DateTime date)
        {
            var value = _stockManager.GetStockForProductAtDate(productId, date);
            return _gridService.DisplayDataInGrid(new object[] {date.ToShortDateString(), value}, GetColumnsForProductStockWithDate(productId));
        }

        public IGridView DisplayStockVariationForProduct(string productId, DateTime startDate, DateTime endDate)
        {
            var value = _stockManager.GetStockVariationForProduct(productId, startDate, endDate);
            var values = new object[] { $"[{startDate.ToShortDateString()}, {endDate.ToShortDateString()}]", value };

            return _gridService.DisplayDataInGrid(values, GetColumnsForProductStockWithDate(productId));
        }

        public IGridView DisplayTotalProductsInStock()
        {
            var value = _stockManager.GetTotalProductsInStock();
            return _gridService.DisplayDataInGrid(value, GetColumnsForForTotalStock());
        }

        private IReadOnlyList<GridColumn> GetColumnsForMovementStock()
        {
            return new[]
            {
                new GridColumn("Date"),
                new GridColumn("Label"),
                new GridColumn("Quantity"),
                new GridColumn("Product")
            };
        }

        private IReadOnlyList<GridColumn> GetColumnsForForProductStock(string productId)
        {
            return new GridColumn[] { new GridColumn(productId) };
        }

        private IReadOnlyList<GridColumn> GetColumnsForForAllProductStocks(ICollection<string> keys)
        {
            return keys.Select(item => new GridColumn(item)).ToList();
        }

        private IReadOnlyList<GridColumn> GetColumnsForProductStockWithDate(string productId)
        {
            return new[]
            {
                new GridColumn(string.Empty),
                new GridColumn(productId)
            };
        }

        private IReadOnlyList<GridColumn> GetColumnsForForTotalStock()
        {
            return new[] {
                new GridColumn("Total")
            };
        }
    }
}
