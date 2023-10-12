using FnacDarty.JobInterview.Stock.Entities;
using System;

namespace FnacDarty.JobInterview.Stock.Validators
{
    internal class AddStockValidator : IValidator<StockMovement>
    {
        private DateTime? _lastInventoryDate;

        public AddStockValidator(DateTime? lastInventoryDate)
        {
            this._lastInventoryDate = lastInventoryDate;
        }

        public ValidatorResult Validate(StockMovement entity)
        {
            var value = _lastInventoryDate.HasValue && entity.Date <= _lastInventoryDate.Value;
            var error = new ArgumentException($"[{entity.Date.ToShortDateString()}, {entity.Label}] : Impossible d'ajouter un mouvement avant la date d'inventaire {_lastInventoryDate?.ToShortDateString()}");

            return new ValidatorResult(!value, !value ? null : error);
        }
    }
}
