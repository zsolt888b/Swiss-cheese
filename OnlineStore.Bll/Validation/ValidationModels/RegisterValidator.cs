using FluentValidation;
using OnlineStore.Core.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Bll.Validation.ValidationModels
{
    public class RegisterValidator : AbstractValidator<RegistrationModel>
    {
        public RegisterValidator()
        {
            RuleFor(model => model.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MaximumLength(128)
                .WithMessage("Password is too long.");

            RuleFor(model => model.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MaximumLength(128)
                .WithMessage("Username is too long.");

            RuleFor(model => model.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .MaximumLength(128)
                .WithMessage("Email is too long.");

            RuleFor(model => model.TelephoneNumber)
                .NotEmpty()
                .WithMessage("Telephone number is required.")
                .MaximumLength(128)
                .WithMessage("Telephone number is too long.");
        }
    }
}
