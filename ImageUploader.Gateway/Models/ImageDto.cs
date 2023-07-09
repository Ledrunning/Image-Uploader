using System;
using ImageUploader.Gateway.Enum;

namespace ImageUploader.Gateway.Models
{
    public class ImageDto : BaseDto
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public double FileSize { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }
        public string LastPhotoName { get; set; }
        public FileUpdate FileUpdate { get; set; }
    }
}