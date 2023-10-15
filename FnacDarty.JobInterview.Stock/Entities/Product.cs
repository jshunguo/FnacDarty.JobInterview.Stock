using System;

namespace FnacDarty.JobInterview.Stock.Entities
{
    /// <summary>
    /// Représente un produit avec un identifiant unique.
    /// </summary>
    public struct Product : IEquatable<Product>
    {
        /// <summary>
        /// Obtient l'identifiant unique du produit.
        /// </summary>
        public string Id { get; }

        public Product(string id)
        {
            Id = id;
        }

        bool IEquatable<Product>.Equals(Product other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is Product)
            {
                var other = (Product)obj;
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
