using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Bll.File;
using OnlineStore.Core.File;
using OnlineStore.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [Authorize(Roles = "User" + "," + "Administrator")]
        [HttpPost]
        public async Task Upload([FromForm]UploadModel uploadModel)  
        {
            await fileService.Upload(uploadModel);
        }

        [Authorize(Roles = "User" + "," + "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Download(int fileId)
        {
            (byte[] file, string filename) = await fileService.Download(fileId);

            return File(file, "application/octet-stream", filename);
        }

        [HttpGet]
        public async Task<List<FileModel>> GetFiles([FromQuery]FileSearchModel fileSearchModel)
        {
            return await fileService.GetFiles(fileSearchModel);
        }
    }
}
