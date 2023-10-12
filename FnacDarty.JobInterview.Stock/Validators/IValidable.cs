using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock.Validators
{
    public interface IValidable<T> 
    {
        IEnumerable<IValidator<T>> GetValidators();
    }
}
