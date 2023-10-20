using System;
using System.Collections.Generic;
using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    /// <summary>
    /// Fournit un ensemble de méthodes pour gérer et accéder aux mouvements de stock dans le système.
    /// </summary>
    public interface IStockMovementRepository 
    {
        /// <summary>
        /// Ajoute un mouvement de stock au système.
        /// </summary>
        /// <param name="stockMovement"></param>
        void AddMovement(StockMovement stockMovement);

        /// <summary>
        /// Ajoute une liste de mouvements de stock au système
        /// </summary>
        /// <param name="stockMovements"></param>
        /// <returns></returns>
        int AddMovements(IEnumerable<StockMovement> stockMovements);

        /// <summary>
        /// Recupère les mouvements de stock d'un produit à une date spécifique
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        IEnumerable<StockMovement> GetProductMovementsForDate(string productId, DateTime date);

        /// <summary>
        /// Recupère tous les mouvements de stock pour une date specifique
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IEnumerable<StockMovement> GetMovementsForDate(DateTime date);

        /// <summary>
        /// Recupère tous les mouvements de stock pour un produit
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        IEnumerable<StockMovement> GetMovementsForProduct(string productId);

        /// <summary>
        /// Recupère le dernier mouvement d'inventaire pour un produit à la date actuelle
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        StockMovement? GetLatestInventoryMovementForProduct(string productId);

        /// <summary>
        /// Recupère les mouvements de stock d'un produit spécifique entre deux dates
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<StockMovement> GetProductMovementsBetweenDates(string productId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Récupère les mouvements d'inventaire les plus récents pour une liste de produits à la date actuelle.
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        IDictionary<string, StockMovement?> GetLatestInventoryMovementsUpToDate(IEnumerable<string> productIds);
    }
}
