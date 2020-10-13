using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Dal.Entities
{
    public class File
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string Filename { get; set; }
    }
}
