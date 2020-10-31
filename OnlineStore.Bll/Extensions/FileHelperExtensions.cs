using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Bll.Extensions
{
    public static class FileHelperExtensions
    {
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile formFile)
        {
            if (formFile == null)
                return null;

            using (var ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
