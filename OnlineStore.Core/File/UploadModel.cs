using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.File
{
    public class UploadModel
    {
        [FromForm]
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public int Price { get; set; }
    }
}
