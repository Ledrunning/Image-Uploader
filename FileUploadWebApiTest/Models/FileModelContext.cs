using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadWebApiTest.Models
{
    public class FileModelContext : DbContext
    {
        public DbSet<FileModel> Files { get; set; }
        public FileModelContext(DbContextOptions<FileModelContext> options)
            : base(options)
        { }
    }
}
