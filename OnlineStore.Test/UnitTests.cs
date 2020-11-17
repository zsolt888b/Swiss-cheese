using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineStore.Bll.File;
using OnlineStore.Bll.Mapper;
using OnlineStore.Bll.User;
using OnlineStore.Core.File;
using OnlineStore.Core.Options;
using OnlineStore.Dal;
using OnlineStore.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OnlineStore.Test
{
    public class UnitTests : IDisposable
    {
        public OnlineStoreDbContext context;
        public IUserService service;
        public IFileService fileService;

        public UnitTests()
        {
            var services = new TestServiceBase();

            var serviceProvider = services.BuildServiceProvider();

            context = serviceProvider.GetService<OnlineStoreDbContext>();

            var accessMock = new AccessMock(context);

            var mapper = MapperConfig.ConfigureAutoMapper();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<UserService>();

            var someOptions = Options.Create(new JwtOptions());

            service = new UserService(null, null, null, someOptions, context, mapper, accessMock, logger);

            var fileLogger = factory.CreateLogger<FileService>();

            fileService = new FileService(accessMock, context, mapper, fileLogger);

            var testUser = new User
            {
                UserName = "TestUser",
                Email = "testuser@example.com",
                PhoneNumber = "+36123456789",
            };

            if (context.ApplicationUsers.Count() < 1)
            {
                context.ApplicationUsers.Add(testUser);
            }

            context.SaveChanges();
        }

        [Fact]
        public async Task GetUser()
        {
            var userModel = await service.GetUser();


            Assert.Equal("TestUser", userModel.UserName);
            Assert.Equal("testuser@example.com", userModel.Email);
            Assert.Equal("+36123456789", userModel.PhoneNumber);
        }

        [Fact]
        public async Task GetUsers()
        {
            var userModels = await service.GetUsers();

            Assert.Equal("TestUser", userModels[0].UserName);
            Assert.Equal("testuser@example.com", userModels[0].Email);
            Assert.Equal("+36123456789", userModels[0].PhoneNumber);
        }

        [Fact]
        public async Task EditUsers()
        {
            var userModels = await service.GetUsers();

            foreach(var user in userModels)
            {
                Assert.False(user.Banned);
            }

            userModels.ForEach(u => u.Banned=true);

            await service.EditUsers(userModels);

            userModels = await service.GetUsers();

            foreach (var user in userModels)
            {
                Assert.True(user.Banned);
            }
        }

        [Fact]
        public async Task FileUploadCommentThanDelete()
        {
            await fileService.Upload(new UploadModel
            {
                Description = "123",
                File = null,
                Filename = "123",
                Price = 123
            });

            var file = context.Files.FirstOrDefault();

            await fileService.Comment(new NewCommentModel { FileId = file.Id, Text = "comment" });

            var fileComments = await fileService.GetCommentsForFile(file.Id);

            Assert.Equal("comment", fileComments[0].Text);
            Assert.Equal("TestUser", fileComments[0].Uploader);

            var comment = context.Comments.FirstOrDefault();

            await fileService.DeleteComment(comment.Id);

            Assert.Equal("Removed by Moderator.", comment.Text);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
