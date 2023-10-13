using System.Collections.Generic;
using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    /// <summary>
    /// Fournit un ensemble de méthodes pour gérer et accéder aux produits dans le système.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Ajoute un produit unique au système.
        /// </summary>
        /// <param name="product">Le produit à ajouter.</param>
        void AddProduct(Product product);

        /// <summary>
        /// Ajoute une collection de produits au système.
        /// </summary>
        /// <param name="products">Les produits à ajouter.</param>
        void AddProducts(IEnumerable<Product> products);

        /// <summary>
        /// Récupère un produit en fonction de son identifiant.
        /// </summary>
        /// <param name="productId">L'identifiant du produit.</param>
        /// <returns>Le produit correspondant à l'identifiant fourni ou null si aucun produit n'est trouvé.</returns>
        Product FindProductById(string productId);

        /// <summary>
        /// Récupère les identifiants des produits existants parmi ceux fournis.
        /// </summary>
        /// <param name="productIds">Une liste d'identifiants de produits à vérifier.</param>
        /// <returns>Une liste d'identifiants correspondant aux produits existants dans le système.</returns>
        IEnumerable<string> FilterExistingProductIds(IEnumerable<string> productIds);

        /// <summary>
        /// Vérifie si un produit spécifique existe dans le système.
        /// </summary>
        /// <param name="productId">L'identifiant du produit à vérifier.</param>
        /// <returns>True si le produit existe, false sinon.</returns>
        bool IsProductExisting(string productId);
    }

}
