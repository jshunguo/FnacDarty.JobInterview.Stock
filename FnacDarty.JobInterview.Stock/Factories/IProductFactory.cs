using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Factories
{
    /// <summary>
    /// Interface définissant une factory pour la création d'objets de type <seealso cref="Product"/>.
    /// </summary>
    public interface IProductFactory
    {
        /// <summary>
        /// Obtient un produit basé sur son identifiant.
        /// </summary>
        /// <param name="productId">L'identifiant EAN du produit à récupérer.</param>
        /// <returns>Un <seealso cref="Product"/> correspondant à l'identifiant fourni.</returns>
        Product Get(string productId);
    }
}
