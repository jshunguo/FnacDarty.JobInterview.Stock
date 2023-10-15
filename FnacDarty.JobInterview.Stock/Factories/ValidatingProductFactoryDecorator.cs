using FnacDarty.JobInterview.Stock.Entities;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public class ValidatingProductFactoryDecorator : IProductFactory
    {
        private readonly IProductFactory _innerFactory;
        private readonly IValidatorFactory _validatorFactory;

        public ValidatingProductFactoryDecorator(IProductFactory innerFactory, IValidatorFactory validatorFactory)
        {
            _innerFactory = innerFactory;
            _validatorFactory = validatorFactory;
        }

        public Product Get(string productId)
        {
            var product = _innerFactory.Get(productId);
            var validator = _validatorFactory.GetProductValidator();

            var validationResult = validator.Validate(product);

            if (!validationResult.IsValid)
            {
                throw validationResult.Throws;
            }

            return product;
        }
    }

}
