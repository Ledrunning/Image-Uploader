using System;

namespace ImageUploader.Gateway.Repository.Entity
{
    public class FileEntity : BaseEntity
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string PhotoPath { get; set; }
    }
}