using System.ComponentModel.DataAnnotations;

namespace ImageUploader.Gateway.Repository.Entity
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}