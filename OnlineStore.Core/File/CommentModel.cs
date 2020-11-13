using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.File
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Uploader { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
