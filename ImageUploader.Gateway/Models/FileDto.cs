using System;

namespace ImageUploader.Gateway.Models
{
    public class FileDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }
    }
}