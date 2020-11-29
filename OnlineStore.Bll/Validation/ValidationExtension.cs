using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineStore.Bll.Validation
{
    public static class ValidationExtension
    {
        public static IEnumerable<ValidationMessage> Transform(this ValidationResult result) =>
            result?.Errors?.Select(e => new ValidationMessage
            {
                PropertyName = e.PropertyName,
                Message = e.ErrorMessage,
                Type = e.Severity.TransformToValidationMessageType()
            });

        public static ValidationMessageType TransformToValidationMessageType(this Severity severity)
        {
            switch (severity)
            {
                case Severity.Error:
                    return ValidationMessageType.Error;
                case Severity.Warning:
                    return ValidationMessageType.Warning;
                case Severity.Info:
                    return ValidationMessageType.Info;
                default:
                    return ValidationMessageType.Info;
            }
        }
    }
}
