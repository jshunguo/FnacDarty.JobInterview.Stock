using FnacDarty.JobInterview.Stock.Validators;
using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Entities
{
    /// <summary>
    /// Représentation un produit avec un identifiant (EAN) et d'autres propriete pour d'ecrire le produit
    /// </summary>
    public struct Product : IEquatable<string>, IValidable<Product>
    {
        /// <summary>
        /// Obtient l'identifiant EAN (8 charactères) du <seealso cref="Product"/>
        /// </summary>
        public string Id { get; }

        public Product(string id)
        {
            Id = id;
        }

        public IEnumerable<IValidator<Product>> GetValidators()
        {
            return new[] { new EanValidator() };
        }

        bool IEquatable<string>.Equals(string other)
        {
            return Equals(other);
        }

        public override bool Equals(object obj)
        {
            var other = (Product)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
