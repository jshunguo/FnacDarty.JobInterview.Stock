using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.Views
{
    public sealed class Client : IClient
    {
        private readonly IStockManager _stockManager;

        public Client(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        private IGridView CreateView(IEnumerable columns, IEnumerable values)
        {
            var definition =  GridView.Definition;

            foreach(var column in columns)
            {
                definition.AddColumn(column.ToString(), column.GetType());
            }

            var view = definition.CreateView();

            view.Bind(values);

            return view;
        }

        public IGridView GetCurrentStockForProduct(string productId)
        {
            var value = _stockManager.GetCurrentStockForProduct(productId);

            return CreateView(new[] {string.Empty, productId }, new object[] { DateTime.UtcNow.Date, value });
        }

        public IGridView GetProductsInStock()
        {
            var products = _stockManager.GetProductsInStock();

            var columns = new List<string> { string.Empty };
            columns.AddRange(products.Keys);

            var values = new List<object> { DateTime.UtcNow.Date };
            values.AddRange(products.Values.Cast<object>());

            return CreateView(columns, values);
        }

        public IGridView GetStockForProductAtDate(string productId, DateTime date)
        {
            var value = _stockManager.GetStockForProductAtDate(productId, date);

            return CreateView(new[] { string.Empty, productId }, new object[] { date, value });
        }

        public IGridView GetStockVariationForProduct(string productId, DateTime startDate, DateTime endDate)
        {
            var startValue = _stockManager.GetStockForProductAtDate(productId, startDate);
            var endValue = _stockManager.GetStockForProductAtDate(productId, endDate);

            var actualValue = _stockManager.GetStockVariationForProduct(productId, startDate, endDate);

            return CreateView(new object [] { string.Empty, startDate, endDate, "Expected_Variation", "Actual_Variation" }, new object[] { productId, startValue, endValue, endValue - startValue, actualValue });
        }

        public IGridView GetTotalProductsInStock()
        {
            var stockValue = _stockManager.GetTotalProductsInStock();

            return CreateView(new object[] { string.Empty, "Total" }, new object[] { DateTime.UtcNow.Date, stockValue });
        }
    }
}
