using FluentValidation;
using OnlineStore.Core.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Bll.Validation.ValidationModels
{
    public class CommentValidator : AbstractValidator<NewCommentModel>
    {
        public CommentValidator()
        {
            RuleFor(model => model.Text)
                .NotEmpty()
                .WithMessage("Comment text is required.")
                .MaximumLength(128)
                .WithMessage("Comment text is too long.");
        }
    }
}
