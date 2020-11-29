using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Bll.Validation
{
    public class ModelValidator : IModelValidator
    {
        private IServiceProvider ServiceProvider { get; }

        public ModelValidator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IEnumerable<ValidationMessage> Validate<T>(T model, string ruleSet = null)
            => ServiceProvider.GetRequiredService<IValidator<T>>()
                .Validate(model, ruleSet: ruleSet)
                .Transform();

        public void ValidateAndThrow<T>(T model, string ruleSet = null)
        {
            var validationErrors = this.Validate(model, ruleSet);
            var errorString = String.Empty;
            if (validationErrors.Any())
            {
                foreach(var error in validationErrors)
                {
                    errorString = errorString + error.PropertyName + " : " + error.Message;
                }

                throw new Exception(errorString);
            }
                
        }
    }
}
