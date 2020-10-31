using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.File
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
