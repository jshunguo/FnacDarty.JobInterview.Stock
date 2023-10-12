using System;

namespace FnacDarty.JobInterview.Stock.Views
{
    /// <summary>
    /// Represente le client de la solution
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Obtient le stock d'un produit à une date spécifique.
        /// </summary>
        /// <param name="productId">L'identifiant du produit unique du stock</param>
        /// <param name="date"></param>
        /// <returns>La quantité du produit en st</returns>
        IGridView GetStockForProductAtDate(string productId, DateTime date);

        /// <summary>
        /// Obtient la variation de stock d'un produit pendant une période spécifiée.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IGridView GetStockVariationForProduct(string productId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtient le stock actuel d'un produit.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IGridView GetCurrentStockForProduct(string productId);

        /// <summary>
        /// Obtient la liste des produits actuellement en stock.
        /// </summary>
        /// <returns></returns>
        IGridView GetProductsInStock();

        /// <summary>
        /// Obtient le nombre total de produits en stock.
        /// </summary>
        /// <returns></returns>
        IGridView GetTotalProductsInStock();
    }
}
