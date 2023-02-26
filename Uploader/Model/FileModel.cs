using System;

namespace Uploader.Model
{
    public class FileModel
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Photo { get; set; }
    }
}