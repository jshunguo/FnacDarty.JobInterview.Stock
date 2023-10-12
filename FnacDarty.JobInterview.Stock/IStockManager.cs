using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Views;
using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock
{
    /// <summary>
    /// Gestionnaire global de tous les mouvements 
    /// </summary>
    public interface IStockManager
    {
        /// <summary>
        /// Ajoute un mouvement de stock pour un produit
        /// </summary>
        /// <param name="date"></param>
        /// <param name="label"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        void AddStock(DateTime date, string label, string productId, long quantity);

        /// <summary>
        /// Ajoute des mouvements de stock pour plusieurs produits à une date donnée
        /// </summary>
        /// <param name="date"></param>
        /// <param name="label"></param>
        /// <param name="products"></param>
        void AddMultipleStock(DateTime date, string label, IDictionary<string,long> products);

        /// <summary>
        /// Obtient le stock d'un produit à une date spécifique.
        /// </summary>
        /// <param name="productId">L'identifiant du produit unique du stock</param>
        /// <param name="date"></param>
        /// <returns>La quantité du produit en st</returns>
        long GetStockForProductAtDate(string productId, DateTime date);

        /// <summary>
        /// Obtient la variation de stock d'un produit pendant une période spécifiée.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        long GetStockVariationForProduct(string productId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtient le stock actuel d'un produit.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        long GetCurrentStockForProduct(string productId);

        /// <summary>
        /// Obtient la liste des produits actuellement en stock.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, long> GetProductsInStock();

        /// <summary>
        /// Obtient le nombre total de produits en stock.
        /// </summary>
        /// <returns></returns>
        long GetTotalProductsInStock();

        /// <summary>
        /// Régularise le stock d'un produit.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        void RegularizeStockForProduct(string productId, long quantity);
    }
}
