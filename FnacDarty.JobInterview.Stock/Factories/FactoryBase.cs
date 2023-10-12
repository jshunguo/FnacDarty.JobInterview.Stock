using FnacDarty.JobInterview.Stock.Entities;
using FnacDarty.JobInterview.Stock.Validators;
using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Factories
{
    public abstract class FactoryBase<TEntity>
    {
        protected virtual void ValidateAndThrows(TEntity entity, IEnumerable<IValidator<TEntity>> validators)
        {
            var queue = new Queue<Exception>();
            foreach (var validator in validators)
            {
                var result = validator.Validate(entity);
                if (!result.IsValid)
                {
                    queue.Enqueue(result.Throws);
                }
            }
            if (queue.Count > 0)
            {
                throw new AggregateException(queue.ToArray());
            }
        }
    }
}
