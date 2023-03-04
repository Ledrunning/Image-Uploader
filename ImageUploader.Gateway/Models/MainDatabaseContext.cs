using ImageUploader.Gateway.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace ImageUploader.Gateway.Models
{
    public class MainDatabaseContext : DbContext
    {
        public MainDatabaseContext(DbContextOptions<MainDatabaseContext> options)
            : base(options)
        {
        }

        public MainDatabaseContext()
        {
        }

        public DbSet<FileEntity> Files { get; set; }
    }
}