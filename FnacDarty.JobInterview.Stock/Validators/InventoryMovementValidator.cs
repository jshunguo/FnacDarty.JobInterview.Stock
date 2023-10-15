using FnacDarty.JobInterview.Stock.Entities;
using System;


namespace FnacDarty.JobInterview.Stock.Validators
{
    public class InventoryMovementValidator : IValidator<StockMovement>
    {
        private StockMovement lastInventory;

        public InventoryMovementValidator(StockMovement lastInventory)
        {
            this.lastInventory = lastInventory;
        }

        public ValidatorResult Validate(StockMovement entity)
        {
            if (entity.Quantity < 0)
            {
                return new ValidatorResult(false, new ArgumentException($"[Inventory:{entity.Date.ToShortDateString()}] : Les mouvements d'inventaire ne peuvent pas avoir une quantité négative ou null."));
            }

            var value = lastInventory.Label == entity.Label &&
                        lastInventory.Date == entity.Date &&
                        lastInventory.Product.Id == entity.Product.Id;
            
            return new ValidatorResult(!value, value ? new ArgumentException($"[Inventory:{entity.Date.ToShortDateString()}] : Il existe déjà un mouvement d'inventaire pour ce produit {entity.Product.Id}.") : null);
        }
    }
}
