using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Dal;
using OnlineStore.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Api.Initializer
{
    public static class DbInitializer
    {
        private static UserManager<Dal.Entities.User> userManager;
        private static RoleManager<IdentityRole> roleManager;
        private static OnlineStoreDbContext dbContext;

        public static void Initialize(OnlineStoreDbContext context, 
            UserManager<Dal.Entities.User> uManager, 
            RoleManager<IdentityRole> rManager)
        {
            dbContext = context;
            userManager = uManager;
            roleManager = rManager;

            context.Database.Migrate();

            if (context.ApplicationUsers.Any())
            {
                return;
            }

            SeedAdmins();
        }

        public static void SeedAdmins()
        {
            var adminUser = new User
            {
                UserName = "Adminuser",
                Email = "admin@example.com",
                PhoneNumber = "+36123456789",
            };

            var adminPassword = "Adminuser123";
            var adminRole = new IdentityRole("Administrator");

            var result1 = userManager.CreateAsync(adminUser, adminPassword).Result;
            var result2 = roleManager.CreateAsync(adminRole).Result;
            var result3 = userManager.AddToRoleAsync(adminUser, adminRole.Name).Result;
        }
    }
}
