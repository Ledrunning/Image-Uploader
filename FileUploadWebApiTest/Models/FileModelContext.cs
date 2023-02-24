using Microsoft.EntityFrameworkCore;

namespace FileUploadWebApiTest.Models
{
    public class FileModelContext : DbContext
    {
        public FileModelContext(DbContextOptions<FileModelContext> options)
            : base(options)
        {
        }

        public FileModelContext()
        {
        }

        public DbSet<FileModel> Files { get; set; }
    }
}