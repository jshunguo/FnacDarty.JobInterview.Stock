using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Validators;
using System;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public class ValidatingStockMovementFactoryDecorator : IStockMovementFactory
    {
        private readonly IStockMovementFactory _innerFactory;
        private readonly IValidatorFactory _validatorFactory;

        public ValidatingStockMovementFactoryDecorator(IStockMovementFactory innerFactory, IValidatorFactory validatorFactory)
        {
            _innerFactory = innerFactory;
            _validatorFactory = validatorFactory;
        }

        public StockMovement Get(StockMovement? lastInventory, DateTime date, string productId, long quantity)
        {
            return Get(lastInventory,date,null,productId,quantity);
        }

        public StockMovement Get(StockMovement? lastInventory, DateTime date, string label, string productId, long quantity)
        {
            var stockMovement = _innerFactory.Get(lastInventory, date, label, productId, quantity);

            IValidator<StockMovement> validator;
            if (stockMovement.IsInventory)
            {
                validator = _validatorFactory.GetInventoryMovementValidator(lastInventory);
            }
            else
            {
                validator = _validatorFactory.GetRegularMovementValidator(lastInventory?.Date);
            }

            var validationResult = validator.Validate(stockMovement);
            if (!validationResult.IsValid)
            {
                throw validationResult.Throws;
            }

            return stockMovement;
        }
    }
}
