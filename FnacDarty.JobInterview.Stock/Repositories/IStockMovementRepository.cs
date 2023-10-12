using System;
using System.Collections.Generic;
using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    /// <summary>
    /// Repository des mouvements de stock
    /// </summary>
    public interface IStockMovementRepository 
    {
        /// <summary>
        /// Ajoute un mouvement d'un ou plusieurs produits 
        /// </summary>
        /// <param name="stockMovement"></param>
        void AddMovement(StockMovement stockMovement);

        /// <summary>
        /// Lister les mouvements de stock pour un produit à une date spécifique
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        IEnumerable<StockMovement> GetByProductAndDate(string productId, DateTime date);

        /// <summary>
        /// Lister tous les mouvements de stock pour une date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IEnumerable<StockMovement> GetByDate(DateTime date);

        /// <summary>
        /// Lister tous les mouvements de stock pour un produit
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        IEnumerable<StockMovement> GetByProduct(string productId);

        /// <summary>
        /// Obtenir le dernier mouvement d'inventaire pour un produit
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        StockMovement GetLatestInventoryForProduct(string productId);
    }
}
