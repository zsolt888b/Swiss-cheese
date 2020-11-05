using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Bll.User;
using OnlineStore.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task Register([FromBody]RegistrationModel model)
        {
            await userService.Register(model);
        }

        [HttpPost]
        public async Task<string> Login([FromBody] LoginModel model)
        {
            return await userService.Login(model);
        }

        [HttpPost]
        public async Task Logout()
        {
            await userService.Logout(); 
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<List<UserModel>> GetUsers()
        {
            return await userService.GetUsers();
        }

    }
}
