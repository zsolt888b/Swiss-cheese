using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Dal.Entities
{
    public class User : IdentityUser
    {
        public int Money { get; set; }
        public double Vat { get; set; }
        public bool Banned { get; set; }
        public ICollection<File> UserFiles { get; set; }
    }
}
