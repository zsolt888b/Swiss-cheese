using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<FileService> logger;

        public FileService(IUserAccess userAccess,
                           OnlineStoreDbContext dbContext,
                           IMapper mapper,
                           ILogger<FileService> logger)
        {
            this.userAccess = userAccess;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task Comment(NewCommentModel model)
        {
            var user = await userAccess.GetUser();

            if (user.Banned)
            {
                throw new Exception("Can't post while banned!");
            }

            var newComment = new Dal.Entities.Comment
            {
                FileId = model.FileId,
                Uploader = user.UserName,
                UploadTime = DateTime.Now,
                Text = model.Text
            };

            dbContext.Comments.Add(newComment);

            await dbContext.SaveChangesAsync();

            logger.LogInformation(user.Id + ": commented on a file!(ID:" + model.FileId + ")");
        }

        public async Task DeleteFile(int fileId)
        {
            var user = await userAccess.GetUser();

            var file = await dbContext.Files
                .Include(f => f.Comments)
                .FirstAsync(f => f.Id == fileId);

            dbContext.Comments.RemoveRange(file.Comments);

            await dbContext.SaveChangesAsync();

            dbContext.Remove(file);

            await dbContext.SaveChangesAsync();

            logger.LogInformation(user.Id + ": deleted a file!(ID:" + fileId + ")");
        }

        public async Task DeleteComment(int commentId)
        {
            var user = await userAccess.GetUser();

            var comment = await dbContext.Comments.FirstAsync(c => c.Id == commentId);

            comment.Text = "Removed by Moderator.";

            await dbContext.SaveChangesAsync();

            logger.LogInformation(user.Id + ": moderated a comment!(" + comment.Id + ")");
        }

        public async Task<(byte[],string)> Download(int fileId)
        {
            var user = await userAccess.GetUser();

            var file = await dbContext.Files.FirstAsync(f => f.Id == fileId);

            if(user.Money < file.Price)
            {
                logger.LogInformation(user.Id + ": not enough money to download the selected file!(" +file.Filename + ")");
                throw new Exception("Not enough money to download that!");
            }

            user.Money -= file.Price;

            await dbContext.SaveChangesAsync();

            logger.LogInformation(user.Id + ": downloaded a file!(" + file.Filename + ")");

            return (file.Content, file.Filename);
        }

        public async Task<List<CommentModel>> GetCommentsForFile(int fileId)
        {
            var comments = await dbContext.Comments.Where(c => c.FileId == fileId).OrderBy(c => c.UploadTime).ToListAsync();

            var mappedComments = mapper.Map<List<CommentModel>>(comments);

            return mappedComments;
        }

        public async Task<FileModel> GetFileDetails(int fileId)
        {
            var file = await dbContext.Files.FirstAsync(f => f.Id == fileId);

            var mappedFile = mapper.Map<FileModel>(file);

            return mappedFile;
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

            string ext = String.Empty;

            if (fileBytes !=null)
            {
               ext = System.IO.Path.GetExtension(uploadModel.File.FileName);
            }

            dbContext.Files.Add(new Dal.Entities.File
            {
                Filename = uploadModel.Filename+ext,
                Price = uploadModel.Price,
                Description = uploadModel.Description,
                UploadTime = DateTime.Now,
                Content = fileBytes,
                UserId = user.Id
            });

            await dbContext.SaveChangesAsync();

            logger.LogInformation(user.Id + ": uploaded a file!(" + uploadModel.Filename + ")");
        }

        public async Task<List<FileModel>> GetMyFiles()
        {
            var user = await userAccess.GetUser();

            var userFiles = (await dbContext.ApplicationUsers
                                .Include(u => u.UserFiles)
                                .FirstAsync(u => u.Id == user.Id))
                                .UserFiles;

            var mappedFiles = mapper.Map<List<FileModel>>(userFiles);

            return mappedFiles;
        }
    }
}
