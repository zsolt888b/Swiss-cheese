using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Bll.Validation
{
    public interface IModelValidator
    {
        IEnumerable<ValidationMessage> Validate<T>(T model, string ruleSet = null);
        void ValidateAndThrow<T>(T model, string ruleSet = null);
    }
}
