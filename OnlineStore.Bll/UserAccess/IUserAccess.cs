using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Bll.UserAccess
{
    public interface IUserAccess
    {
        Task<Dal.Entities.User> GetUser();
    }
}
