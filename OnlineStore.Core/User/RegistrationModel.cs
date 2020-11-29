using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.User
{
    public class RegistrationModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TelephoneNumber { get; set; }

    }

    public class RegistrationValidator: AbstractValidator<RegistrationModel>
    {
        public RegistrationValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email is required!");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");
            RuleFor(x => x.TelephoneNumber).NotEmpty().WithMessage("Telephone number is required!");
        }
    }
}
