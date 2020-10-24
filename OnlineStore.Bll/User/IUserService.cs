using OnlineStore.Core.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Bll.User
{
    public interface IUserService
    {
        Task Register(RegistrationModel model);
        Task<string> Login(LoginModel model);
        Task Logout();
    }
}
