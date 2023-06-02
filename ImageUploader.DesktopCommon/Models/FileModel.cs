using System;

namespace ImageUploader.DesktopCommon.Models
{
    public class FileModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public byte[] Photo { get; set; }
    }
}