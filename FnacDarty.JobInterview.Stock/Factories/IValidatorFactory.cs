using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Validators;
using System;

namespace FnacDarty.JobInterview.Stock.Factories
{
    /// <summary>
    /// Interface définissant une factory pour obtenir des validateurs spécifiques.
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Obtient le validateur pour les produits.
        /// </summary>
        /// <returns>Un validateur pour les produits.</returns>
        IValidator<Product> GetProductValidator();

        /// <summary>
        /// Obtient le validateur pour les mouvements de stock de type "Inventaire".
        /// </summary>
        /// <param name="lastInventory">Le dernier mouvement de stock de type "Inventaire".</param>
        /// <returns>Un validateur pour les mouvements de stock de type "Inventaire".</returns>
        IValidator<StockMovement> GetInventoryMovementValidator(StockMovement? lastInventory);

        /// <summary>
        /// Obtient le validateur pour les mouvements de stock réguliers.
        /// </summary>
        /// <param name="lastInventoryDate">La date du dernier mouvement de stock de type "Inventaire".</param>
        /// <returns>Un validateur pour les mouvements de stock réguliers.</returns>
        IValidator<StockMovement> GetRegularMovementValidator(DateTime? lastInventoryDate);
    }
}
