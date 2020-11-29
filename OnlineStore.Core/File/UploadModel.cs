using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.File
{
    public class UploadModel
    {
        [FromForm]
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public int Price { get; set; }
    }

    public class UploadValidator : AbstractValidator<UploadModel>
    {
        public UploadValidator()
        {
            RuleFor(x => x.File).NotEmpty().WithMessage("File is required!");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required!");
            RuleFor(x => x.Filename).Must(file => file.EndsWith(".caff") == true).WithMessage("File is not a CAFF file!");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price is required!");
        }
    }
}
