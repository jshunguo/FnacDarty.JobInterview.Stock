using FnacDarty.JobInterview.Stock.Entities;
using System;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.Validators
{
    internal class EanValidator : IValidator<Product>
    {
        public ValidatorResult Validate(Product entity)
        {
            var value = string.IsNullOrEmpty(entity.Id) || entity.Id.Length != 8;
            value = value || entity.Id.Any(c => !char.IsLetterOrDigit(c));

            return new ValidatorResult(!value, !value ? null : new ArgumentException($"L'identifiant du produit {entity.Id} doit comporter 8 charactère alphanumerique"));
        }
    }
}
