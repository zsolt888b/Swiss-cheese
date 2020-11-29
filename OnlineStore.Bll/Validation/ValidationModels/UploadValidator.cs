using FluentValidation;
using OnlineStore.Core.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Bll.Validation.ValidationModels
{
    public class UploadValidator : AbstractValidator<UploadModel>
    {
        public UploadValidator()
        {
            RuleFor(model => model.Filename)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MaximumLength(128)
                .WithMessage("Password is too long.");

            RuleFor(model => model.Price)
                .GreaterThan(-1)
                .WithMessage("Wrong price is given.");

            RuleFor(model => model.Description)
                .NotEmpty()
                .WithMessage("Email is required.")
                .MaximumLength(128)
                .WithMessage("Email is too long.");

            RuleFor(model => model.File)
                .NotNull()
                .WithMessage("File is required.");
        }
    }
}
