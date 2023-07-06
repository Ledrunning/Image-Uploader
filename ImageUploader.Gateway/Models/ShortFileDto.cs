﻿using System;

namespace ImageUploader.Gateway.Models
{
    public class ShortFileDto : BaseDto
    {
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public double FileSize { get; set; }
    }
}