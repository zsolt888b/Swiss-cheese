using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Bll.UserAccess
{
    public class UserAccess : IUserAccess
    {
        private readonly UserManager<Dal.Entities.User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAccess(UserManager<Dal.Entities.User> userManager, 
                          IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Dal.Entities.User> GetUser()
        {
            return await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
        }
    }
}
