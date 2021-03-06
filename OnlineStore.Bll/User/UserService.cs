﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Bll.UserAccess;
using OnlineStore.Bll.Validation;
using OnlineStore.Core.Options;
using OnlineStore.Core.User;
using OnlineStore.Dal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
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
        private readonly OnlineStoreDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IUserAccess userAccess;
        private readonly ILogger<UserService> logger;
        private readonly IModelValidator validator;
        private readonly JwtOptions jwtOptions;

        public UserService(UserManager<Dal.Entities.User> userManager,
            SignInManager<Dal.Entities.User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtOptions> jwtOptions,
            OnlineStoreDbContext dbContext,
            IMapper mapper,
            IUserAccess userAccess,
            ILogger<UserService> logger,
            IModelValidator validator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userAccess = userAccess;
            this.logger = logger;
            this.validator = validator;
            this.jwtOptions = jwtOptions.Value;
        }

        public async Task EditUsers(List<UserModel> users)
        {
            var adminUser = await userAccess.GetUser();

            foreach(var user in users)
            {
                var applicationUser = await dbContext.ApplicationUsers.FirstAsync(u => u.Id == user.Id);

                if(user.Money!=applicationUser.Money || user.Vat!=applicationUser.Vat || user.Banned != applicationUser.Banned)
                {
                    applicationUser.Banned = user.Banned;
                    applicationUser.Money = user.Money;
                    applicationUser.Vat = user.Vat;

                    await dbContext.SaveChangesAsync();

                    logger.LogInformation(adminUser.Id + ": edited a user(" + user.Id + ")");
                }
            }
        }

        public async Task<bool> GetRole()
        {
            var user = await userAccess.GetUser();

            var roles = await userManager.GetRolesAsync(user);

            return roles.Contains("Administrator");
        }

        public async Task<UserModel> GetUser()
        {
            var user = await userAccess.GetUser();

            var userMapped = mapper.Map<UserModel>(user);

            return userMapped;
        }

        public async Task<List<UserModel>> GetUsers()
        {
            var users = await dbContext.ApplicationUsers.ToListAsync();

            var usersMapped = mapper.Map<List<UserModel>>(users);

            return usersMapped;
        }

        public async Task<string> Login(LoginModel model)
        {
            validator.ValidateAndThrow(model);

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
            validator.ValidateAndThrow(model);

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
                logger.LogInformation(userReg.Id + ": registered!");
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error during registration!");
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
                logger.LogInformation(model.Username + ": no such user found in the database!");
                throw new Exception("No such user found in the database!");
            }
        }
    }
}
