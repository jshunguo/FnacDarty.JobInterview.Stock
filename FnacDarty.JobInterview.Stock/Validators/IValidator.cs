using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.JobInterview.Stock.Validators
{
    public interface IValidator<in T> 
    {
        ValidatorResult Validate(T entity);
    }

    public struct ValidatorResult
    {
        public bool IsValid { get; }
        public Exception Throws { get; }

        public ValidatorResult(bool isValid, Exception throws)
        {
            IsValid = isValid;
            Throws = throws;
        }
    }
}
