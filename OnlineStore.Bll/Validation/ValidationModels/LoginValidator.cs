using FluentValidation;
using OnlineStore.Core.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Bll.Validation.ValidationModels
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
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
        }
    }
}
