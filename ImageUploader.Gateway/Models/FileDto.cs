using System;

namespace ImageUploader.Gateway.Models
{
    public class FileDto : BaseDto
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }
        public string LastPhotoName { get; set; }
        public bool IsUpdated { get; set; }
    }
}