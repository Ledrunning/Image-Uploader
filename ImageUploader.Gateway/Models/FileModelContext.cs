using ImageUploader.Gateway.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace ImageUploader.Gateway.Models
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

        public DbSet<FileEntity> Files { get; set; }
    }
}