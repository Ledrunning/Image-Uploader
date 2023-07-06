using System;

namespace ImageUploader.DesktopCommon.Models
{
    public class FileDto : BaseFile
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public double FileSize { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }
        public string LastPhotoName { get; set; }
        public bool IsUpdated { get; set; }
    }
}