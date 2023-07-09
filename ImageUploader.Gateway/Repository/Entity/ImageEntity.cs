using System;

namespace ImageUploader.Gateway.Repository.Entity
{
    public class ImageEntity : BaseEntity
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public double FileSize { get; set; }
        public string PhotoPath { get; set; }
    }
}