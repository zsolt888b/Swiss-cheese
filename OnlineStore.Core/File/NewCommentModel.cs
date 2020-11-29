using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.File
{
    public class NewCommentModel
    {
        public int FileId { get; set; }
        public string Text { get; set; }
    }

    public class NewCommentValidator : AbstractValidator<NewCommentModel>
    {
        public NewCommentValidator()
        {
            RuleFor(m => m.FileId).NotEmpty();
            RuleFor(m => m.Text).NotEmpty().WithMessage("Comment is required!");
        }
    }
}
