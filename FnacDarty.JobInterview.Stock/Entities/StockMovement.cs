using FnacDarty.JobInterview.Stock.Validators;
using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Entities
{
    /// <summary>
    /// Représente un mouvement de produit avec date, libellé
    /// </summary>
    public struct StockMovement : IValidable<StockMovement>
    {
        private const string InventoryName = "inventaire";

        /// <summary>
        /// Obtient ou definit l'identifiant du mouvement 
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Obtient ou definit le libellé du <seealso cref="StockMovement"/>
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Obtient ou definit la date d'execution du <seealso cref="StockMovement"/>
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Obtient une valeur qui indique que c'est un inventaire
        /// </summary>
        public bool IsInventory { get;}

        /// <summary>
        /// Obtient le produit du <seealso cref="StockMovement"/>
        /// </summary>
        public Product Product { get; }

        /// <summary>
        /// Obtient la quantité du <seealso cref="StockMovement"/>
        /// </summary>
        public long Quantity { get; }

        public StockMovement(DateTime date, string label, Product product, long quantity)
        {
            Id = Guid.NewGuid();
            Date = date;
            Label = label;
            IsInventory = string.Equals(label, InventoryName, StringComparison.OrdinalIgnoreCase);
            Product = product;
            Quantity = quantity;
        }

        public StockMovement(DateTime date, Product product,long quantity) : this(date, InventoryName, product, quantity)
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is StockMovement)
            {
                var other = (StockMovement)obj;

                return Date.Equals(other.Date) &&
                       Label == other.Label &&
                       IsInventory == other.IsInventory &&
                       Product.Equals(other.Product) &&
                       Quantity == other.Quantity;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Label, IsInventory, Product, Quantity);
        }

        IEnumerable<IValidator<StockMovement>> IValidable<StockMovement>.GetValidators()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IValidator<StockMovement>> GetValidators(StockMovement inventory)
        {
            if(IsInventory)
            {
                return new[] { new InventoryValidator(inventory) };
            }
            else
            {
                return new[] { new AddStockValidator(inventory.Date) };
            }
        }
    }
}
