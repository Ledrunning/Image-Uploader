using System;

namespace ImageUploader.DesktopClient.Model
{
    public class FileModel
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public byte[] Photo { get; set; }
    }
}