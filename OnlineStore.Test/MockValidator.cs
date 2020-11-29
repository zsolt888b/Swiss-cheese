using OnlineStore.Bll.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Test
{
    public class MockValidator : IModelValidator
    {
        public IEnumerable<ValidationMessage> Validate<T>(T model, string ruleSet = null)
        {
            return new List<ValidationMessage>();
        }

        public void ValidateAndThrow<T>(T model, string ruleSet = null)
        {

        }
    }
}
