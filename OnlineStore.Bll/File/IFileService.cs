using OnlineStore.Core.File;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Bll.File
{
    public interface IFileService
    {
        Task Upload(UploadModel uploadModel);
        Task<(byte[],string)> Download(int fileId);
        Task<List<FileModel>> GetFiles(FileSearchModel fileSearchModel);
        Task DeleteFile(int fileId);
        Task Comment(NewCommentModel model);
        Task DeleteComment(int commentId);
        Task<FileModel> GetFileDetails(int fileId);
        Task<List<CommentModel>> GetCommentsForFile(int fileId);
    }
}
