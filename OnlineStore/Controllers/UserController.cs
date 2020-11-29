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
            var validator = new RegistrationValidator();
            var result = validator.Validate(model);
            if(result.IsValid)
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

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task EditUsers([FromBody]List<UserModel> models)
        {
            var validator = new UserValidator();
            bool error = false;
            foreach (UserModel model in models)
            {
                var result = validator.Validate(model);
                if (!result.IsValid)
                {
                    error = true;
                    break;
                }
            }
            if(!error)
                await userService.EditUsers(models);
        }

        [HttpGet]
        [Authorize(Roles = "User" + "," + "Administrator")]
        public async Task<bool> GetRole()
        {
            return await userService.GetRole();
        }

        [HttpGet]
        [Authorize(Roles = "User" + "," + "Administrator")]
        public async Task<UserModel> GetUser()
        {
            return await userService.GetUser();
        }

    }
}
