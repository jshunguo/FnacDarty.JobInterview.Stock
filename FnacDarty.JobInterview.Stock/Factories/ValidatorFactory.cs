using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Validators;
using System;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IValidator<StockMovement> GetInventoryMovementValidator(StockMovement lastInventory)
        {
            return new InventoryMovementValidator(lastInventory);
        }

        public IValidator<Product> GetProductValidator()
        {
            return new ProductValidator();
        }

        public IValidator<StockMovement> GetRegularMovementValidator(DateTime lastInventoryDate)
        {
            return new RegularMovementValidator(lastInventoryDate);
        }
    }
}
