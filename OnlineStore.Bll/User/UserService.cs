using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Core.Options;
using OnlineStore.Core.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Bll.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Dal.Entities.User> userManager;
        private readonly SignInManager<Dal.Entities.User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly JwtOptions jwtOptions;

        public UserService(UserManager<Dal.Entities.User> userManager,
            SignInManager<Dal.Entities.User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.jwtOptions = jwtOptions.Value;
        }
        public async Task<string> Login(LoginModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid login!");
            }

            return await GenerateJwtToken(model);
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task Register(RegistrationModel model)
        {
            var isNameInUse = await userManager.FindByNameAsync(model.Username);
            if (isNameInUse != null)
            {
                throw new Exception("There is a user already registered with the given name!");
            }


            var isEmailInUse = await userManager.FindByEmailAsync(model.Email);
            if (isEmailInUse != null)
            {
                throw new Exception("There is a user already registered with the given email!");
            }
            var userReg = new Dal.Entities.User
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.TelephoneNumber
            };

            var result = await userManager.CreateAsync(userReg, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Something went wrong druing registration!");
            }

            try
            {
                var userRole = new IdentityRole("User");
                await roleManager.CreateAsync(userRole);
                await userManager.AddToRoleAsync(userReg, userRole.Name);
            }
            catch(Exception ex)
            {
                throw new Exception("Internal server error! Exception:",ex);
            }
        }


        private async Task<string> GenerateJwtToken(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {

                var result = await signInManager.CheckPasswordSignInAsync
                                (user, model.Password, lockoutOnFailure: false);

                var roles = await userManager.GetRolesAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception("Invalid login!");
                }

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

                foreach(var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = new JwtSecurityToken
                (
                    expires: DateTime.Now.AddYears(1),
                    notBefore: DateTime.Now,
                    issuer: jwtOptions.Issuer,
                    audience: jwtOptions.Audience,
                    claims: claims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                                (Encoding.UTF8.GetBytes(jwtOptions.Key)),
                            SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                throw new Exception("Something went wrong!");
            }
        }
    }
}
