using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.User
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");
        }
    }
}
