using System;

namespace ImageUploader.DesktopCommon.Models
{
    public class ImageModel : BaseFile
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }

        public DateTimeOffset CreationTime { get; set; }
        public double FileSize { get; set; }
    }
}