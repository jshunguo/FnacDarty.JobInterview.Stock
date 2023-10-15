using FnacDarty.JobInterview.Stock.Entities;
using System;

namespace FnacDarty.JobInterview.Stock.Factories
{
    /// <summary>
    /// Interface définissant une factory pour la création des <seealso cref="StockMovement"/>.
    /// </summary>
    public interface IStockMovementFactory
    {
        /// <summary>
        /// Crée et obtient un mouvement de stock basé sur les paramètres fournis.
        /// </summary>
        /// <param name="lastInventory">Le dernier mouvement de stock de type "Inventaire".</param>
        /// <param name="date">La date du mouvement de stock.</param>
        /// <param name="productId">L'identifiant du produit concerné par le mouvement.</param>
        /// <param name="quantity">La quantité associée au mouvement de stock.</param>
        /// <returns>Un <seealso cref="StockMovement"/> correspondant aux paramètres fournis.</returns>
        StockMovement Get(StockMovement lastInventory, DateTime date, string productId, long quantity);

        /// <summary>
        /// Crée et obtient un mouvement de stock avec un label spécifique basé sur les paramètres fournis.
        /// </summary>
        /// <param name="lastInventory">Le dernier mouvement de stock de type "Inventaire".</param>
        /// <param name="date">La date du mouvement de stock.</param>
        /// <param name="label">Un libellé descriptif pour le mouvement de stock.</param>
        /// <param name="productId">L'identifiant du produit concerné par le mouvement.</param>
        /// <param name="quantity">La quantité associée au mouvement de stock.</param>
        /// <returns>Un <seealso cref="StockMovement"/> correspondant aux paramètres fournis.</returns>
        StockMovement Get(StockMovement lastInventory, DateTime date, string label, string productId, long quantity);
    }
}
