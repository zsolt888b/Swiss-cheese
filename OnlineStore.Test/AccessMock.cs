using Microsoft.EntityFrameworkCore;
using OnlineStore.Bll.UserAccess;
using OnlineStore.Dal;
using OnlineStore.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Test
{
    public class AccessMock : IUserAccess
    {
        private readonly OnlineStoreDbContext dbContext;

        public AccessMock(OnlineStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> GetUser()
        {
            return await dbContext.ApplicationUsers.FirstOrDefaultAsync();
        }
    }
}
