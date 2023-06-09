﻿using System;

namespace ImageUploader.DesktopCommon.Models
{
    public class FileDto : BaseFile
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public byte[] Photo { get; set; }
    }
}