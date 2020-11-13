using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Dal.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Uploader { get; set; }
        public DateTime UploadTime { get; set; }
        public int FileId { get; set; }
        public File File { get; set; }
    }
}
