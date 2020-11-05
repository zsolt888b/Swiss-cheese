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
    }
}
