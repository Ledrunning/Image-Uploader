using System;

namespace ImageUploader.DesktopCommon.Models
{
    public class FileModel : BaseFile
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }
}