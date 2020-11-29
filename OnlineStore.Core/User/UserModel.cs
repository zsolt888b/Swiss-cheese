using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.User
{
    public class UserModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public int Money { get; set; }
        public double Vat { get; set; }
        public bool Banned { get; set; }
        public string Id { get; set; }
    }

    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Money).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Vat).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Banned).NotNull();
            RuleFor(x => x.Id).NotNull();
        }
    }
}
