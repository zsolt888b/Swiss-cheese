﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<string> Upload([FromForm]UploadModel uploadModel)  
        {
            return await fileService.Upload(uploadModel);
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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task DeleFile(int fileId)
        {
            await fileService.DeleteFile(fileId);
        }

        [Authorize(Roles = "User" + "," + "Administrator")]
        [HttpPost]
        public async Task Comment([FromQuery]NewCommentModel model)
        {
            await fileService.Comment(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task DeleteComment(int commentId)
        {
            await fileService.DeleteComment(commentId);
        }

        [HttpGet]
        public async Task<FileModel> GetFileDetails(int fileId)
        {
            return await fileService.GetFileDetails(fileId);
        }

        [HttpGet]
        public async Task<List<CommentModel>> GetCommentsForFile(int fileId)
        {
            return await fileService.GetCommentsForFile(fileId);
        }

        [Authorize(Roles = "User" + "," + "Administrator")]
        [HttpGet]
        public async Task<List<FileModel>> GetMyFiles()
        {
            return await fileService.GetMyFiles();
        }
    }
}
