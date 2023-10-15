using System;
using System.Collections.Generic;
using FnacDarty.JobInterview.Stock.Views;

namespace FnacDarty.JobInterview.Stock
{
    /// <summary>
    /// Interface définissant les opérations du client pour la gestion et l'affichage des stocks et de leurs mouvements.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Affiche le mouvement d'un produit dans le stock à une date donnée.
        /// </summary>
        /// <param name="date">Date du mouvement.</param>
        /// <param name="label">Libellé décrivant le mouvement.</param>
        /// <param name="productId">Identifiant unique du produit concerné.</param>
        /// <param name="quantity">Quantité du produit concernée par le mouvement.</param>
        /// <returns>Une grille affichant les détails du mouvement.</returns>
        IGridView DisplayMovement(DateTime date, string label, string productId, long quantity);

        /// <summary>
        /// Affiche plusieurs mouvements de stock sur différents produits à une date spécifiée.
        /// </summary>
        /// <param name="date">Date des mouvements.</param>
        /// <param name="label">Libellé décrivant les mouvements.</param>
        /// <param name="productQuantities">Dictionnaire associant chaque identifiant de produit à sa quantité mouvementée.</param>
        /// <returns>Une grille affichant les détails des mouvements.</returns>
        IGridView DisplayMultipleStockMovements(DateTime date, string label, IDictionary<string, long> productQuantities);

        /// <summary>
        /// Affiche le stock d'un produit à une date donnée.
        /// </summary>
        /// <param name="productId">Identifiant unique du produit.</param>
        /// <param name="date">Date à laquelle le stock est consulté.</param>
        /// <returns>Une grille affichant le stock du produit à la date donnée.</returns>
        IGridView DisplayStockForProductAtDate(string productId, DateTime date);

        /// <summary>
        /// Affiche la variation du stock d'un produit sur une période donnée.
        /// </summary>
        /// <param name="productId">Identifiant unique du produit.</param>
        /// <param name="startDate">Date de début de la période.</param>
        /// <param name="endDate">Date de fin de la période.</param>
        /// <returns>Une grille affichant la variation du stock du produit pendant la période spécifiée.</returns>
        IGridView DisplayStockVariationForProduct(string productId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Affiche le stock actuel d'un produit.
        /// </summary>
        /// <param name="productId">Identifiant unique du produit.</param>
        /// <returns>Une grille affichant le stock actuel du produit.</returns>
        IGridView DisplayCurrentStockForProduct(string productId);

        /// <summary>
        /// Affiche tous les produits actuellement en stock.
        /// </summary>
        /// <returns>Une grille affichant la liste des produits en stock.</returns>
        IGridView DisplayProductsInStock();

        /// <summary>
        /// Affiche le nombre total de produits en stock.
        /// </summary>
        /// <returns>Une grille affichant la quantité totale de tous les produits en stock.</returns>
        IGridView DisplayTotalProductsInStock();

        /// <summary>
        /// Régularise le stock d'un produit et affiche le nouveau stock après régularisation.
        /// </summary>
        /// <param name="productId">Identifiant unique du produit à régulariser.</param>
        /// <param name="quantity">Quantité à laquelle régulariser le stock du produit.</param>
        /// <returns>Une grille affichant le stock du produit après régularisation.</returns>
        IGridView DisplayStockAfterRegularizationForProduct(string productId, long quantity);
    }

}
