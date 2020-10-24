using OnlineStore.Bll.UserAccess;
using OnlineStore.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Bll.File
{
    public class FileService : IFileService
    {
        private readonly IUserAccess userAccess;
        private readonly OnlineStoreDbContext dbContext;

        public FileService(IUserAccess userAccess,
                           OnlineStoreDbContext dbContext)
        {
            this.userAccess = userAccess;
            this.dbContext = dbContext;
        }

        public async Task Upload()
        {
            var user = await userAccess.GetUser();

            dbContext.Files.Add(new Dal.Entities.File { 
                Filename = user.UserName + "_file",
                UserId = user.Id
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
