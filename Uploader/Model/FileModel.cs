using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uploader.Model
{
    public class FileModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Photo { get; set; }
    }
}
