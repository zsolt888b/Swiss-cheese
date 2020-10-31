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
    }
}
