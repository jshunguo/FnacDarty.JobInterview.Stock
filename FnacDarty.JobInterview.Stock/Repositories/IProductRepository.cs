using System.Collections.Generic;
using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Repositories
{
    /// <summary>
    /// Repository des produits
    /// </summary>
    public interface IProductRepository 
    {
        /// <summary>
        /// Ajoute un produit
        /// </summary>
        /// <param name="product"></param>
        void Add(Product product);

        /// <summary>
        /// Obtent un produit par son identifiant unique EAN
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Product GetById(string productId);

        /// <summary>
        /// Vérifie si un produit existe
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool Exists(string productId);

        /// <summary>
        /// Vérifie si des produits existent
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        bool Exists(IEnumerable<string> productIds);
    }
}
