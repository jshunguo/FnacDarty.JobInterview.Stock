using FnacDarty.JobInterview.Stock.Validators;
using System;

namespace FnacDarty.JobInterview.Stock.Entities
{
    /// <summary>
    /// Représente un mouvement de stock associé à un produit.
    /// </summary>
    public struct StockMovement : IEquatable<StockMovement>
    {
        public const string InventoryName = "inventaire";

        public static StockMovement DefaultInventory = new StockMovement(DateTime.MinValue.ToUniversalTime().Date, null, new Product("EAN1234"), 0);


        /// <summary>
        /// Obtient le libellé descriptif du mouvement de stock.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Obtient la date à laquelle le mouvement de stock a eu lieu.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Indique si le mouvement de stock est de type "Inventaire".
        /// </summary>
        public bool IsInventory { get; }

        /// <summary>
        /// Obtient le produit associé au mouvement de stock.
        /// </summary>
        public Product Product { get; }

        /// <summary>
        /// Obtient la quantité associée au mouvement de stock.
        /// </summary>
        public long Quantity { get; }

        public StockMovement(DateTime date, string label, Product product, long quantity)
        {
            Date = date;
            IsInventory = string.IsNullOrEmpty(label) || string.Equals(label, InventoryName, StringComparison.OrdinalIgnoreCase);
            Product = product;
            Quantity = quantity;
            Label = string.IsNullOrEmpty(label) ? InventoryName : label;
        }

        public StockMovement(DateTime date, Product product, long quantity) : this(date, InventoryName, product, quantity)
        {

        }

        bool IEquatable<StockMovement>.Equals(StockMovement other)
        {
            return Equals(other);
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
    }
}
