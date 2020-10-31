using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Bll.Extensions;
using OnlineStore.Bll.UserAccess;
using OnlineStore.Core.File;
using OnlineStore.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Bll.File
{
    public class FileService : IFileService
    {
        private readonly IUserAccess userAccess;
        private readonly OnlineStoreDbContext dbContext;
        private readonly IMapper mapper;

        public FileService(IUserAccess userAccess,
                           OnlineStoreDbContext dbContext,
                           IMapper mapper)
        {
            this.userAccess = userAccess;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<(byte[],string)> Download(int fileId)
        {
            var user = await userAccess.GetUser();

            var file = await dbContext.Files.FirstAsync(f => f.Id == fileId);

            if(user.Money < file.Price)
            {
                throw new Exception("Not enough monoey to download that!");
            }

            user.Money -= file.Price;

            await dbContext.SaveChangesAsync();

            return (file.Content, file.Filename);
        }

        public async Task<List<FileModel>> GetFiles(FileSearchModel fileSearchModel)
        {
            var files = dbContext
                .Files
                .Where(!String.IsNullOrEmpty(fileSearchModel.Filename), f => f.Filename.Contains(fileSearchModel.Filename));


            var mappedFiles = mapper.Map<List<FileModel>>(files);

            return mappedFiles;
        }

        public async Task Upload(UploadModel uploadModel)
        {
            var user = await userAccess.GetUser();

            var fileBytes = await uploadModel.File.ToByteArrayAsync();

            dbContext.Files.Add(new Dal.Entities.File
            {
                Filename = uploadModel.Filename,
                Price = uploadModel.Price,
                Description = uploadModel.Description,
                UploadTime = DateTime.Now,
                Content = fileBytes,
                UserId = user.Id
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
